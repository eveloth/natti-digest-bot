using Telegram.Bot.Types;

namespace NattiDigestBot.Extensions;

public static class MessageExtensions
{
    public static string? GetCommandArguments(this Message message)
    {
        return message.Text!.Split(' ', 2).ElementAtOrDefault(1);
    }
}