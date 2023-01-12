using NattiDigestBot.Replies;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NattiDigestBot.Extensions;

public static class BotClientExtensions
{
    public static async Task<Message> SendReply(
        this ITelegramBotClient botClient,
        long chatId,
        Reply reply,
        CancellationToken cancellationToken,
        int? replyTo = null
    )
    {
        return await botClient.SendTextMessageAsync(
            chatId,
            reply.ReplyText,
            replyMarkup: reply.ReplyMarkup,
            parseMode: reply.ParseMode,
            replyToMessageId: replyTo,
            cancellationToken: cancellationToken
        );
    }

    public static async Task EditReply(
        this ITelegramBotClient botClient,
        long chatId,
        int messageId,
        Reply reply,
        CancellationToken cancellationToken
    )
    {
        var markup = reply.ReplyMarkup switch
        {
            InlineKeyboardMarkup replyMarkup => replyMarkup,
            _ => null
        };

        await botClient.EditMessageTextAsync(
            chatId,
            messageId,
            reply.ReplyText,
            parseMode: reply.ParseMode,
            replyMarkup: markup,
            cancellationToken: cancellationToken
        );
    }
}