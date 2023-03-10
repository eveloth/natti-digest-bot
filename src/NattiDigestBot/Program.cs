using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using NattiDigestBot;
using NattiDigestBot.Commands;
using NattiDigestBot.Commands.Interfaces;
using NattiDigestBot.Controllers;
using NattiDigestBot.Data;
using NattiDigestBot.Extensions;
using NattiDigestBot.Options;
using NattiDigestBot.Services;
using NattiDigestBot.Services.DbServices;
using NattiDigestBot.State;
using Serilog;
using Serilog.Events;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

var seqOptions = new SeqOptions();
builder.Configuration.Bind(nameof(seqOptions), seqOptions);

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey)
    .WriteTo.Console()
    .CreateLogger();

// Setup Bot configuration
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

builder.Host.UseSerilog();

// Register named HttpClient to get benefits of IHttpClientFactory
// and consume it with ITelegramBotClient typed client.
// More read:
//  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#typed-clients
//  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
builder.Services
    .AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>(
        (httpClient, sp) =>
        {
            BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
            TelegramBotClientOptions options = new(botConfig.BotToken);
            return new TelegramBotClient(options, httpClient);
        }
    );

// Dummy business-logic service
builder.Services.AddScoped<UpdateHandlers>();

builder.Services.AddDbContext<DigestContext>(optionsBuilder =>
{
    optionsBuilder.UseLazyLoadingProxies();
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

// There are several strategies for completing asynchronous tasks during startup.
// Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
// We are going to use IHostedService to add and later remove Webhook
builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<ICommandExecutor, CommandExecutor>();
builder.Services.AddScoped<ICallbackQueryProcessor, CallbackQueryProcessor>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDigestService, DigestService>();

// The Telegram.Bot library heavily depends on Newtonsoft.Json library to deserialize
// incoming webhook updates and send serialized responses back.
// Read more about adding Newtonsoft.Json to ASP.NET Core pipeline:
//   https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0#add-newtonsoftjson-based-json-format-support
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var me = botClient.GetMeAsync().Result;
StateStorage.BotName = me.Username;
Log.Information("Setting bot name to: {BotName}", StateStorage.BotName);

botClient.SetMyCommandsAsync(BotCommands.PrivateChat, BotCommandScope.AllPrivateChats());
botClient.SetMyCommandsAsync(BotCommands.AllChats, BotCommandScope.Default());

using (var scope = app.Services.CreateScope())
{
    var digestContext = scope.ServiceProvider.GetRequiredService<DigestContext>();
    digestContext.Database.Migrate();
}

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

app.UseSerilogRequestLogging();

// Construct webhook route from the Route configuration parameter
// It is expected that BotController has single method accepting Update
app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.MapControllers();
app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable RCS1110 // Declare type inside namespace.
namespace NattiDigestBot
{
    public class BotConfiguration
#pragma warning restore RCS1110 // Declare type inside namespace.
#pragma warning restore CA1050 // Declare types in namespaces
    {
        public static readonly string Configuration = "BotConfiguration";

        public string BotToken { get; init; } = default!;
        public string HostAddress { get; init; } = default!;
        public string Route { get; init; } = default!;
        public string SecretToken { get; init; } = default!;
    }
}