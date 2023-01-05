using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using NattiDigestBot.Filters;
using NattiDigestBot.Services;

namespace NattiDigestBot.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        try
        {
            await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        }
        catch (Exception e)
        {
            await handleUpdateService.HandleErrorAsync(e, cancellationToken);
        }
        return Ok();
    }
}