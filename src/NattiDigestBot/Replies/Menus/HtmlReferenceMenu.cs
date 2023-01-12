using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies.Menus;

public static class HtmlReferenceMenu
{
    public static Reply HtmlReferenceSection { get; } =
        new()
        {
            ReplyText =
                "HTML — это язык разметки. Вообще-то он используется для того, чтобы делать сайты, "
                + "но поскольку боты плохо справляются с тем, чтобы понять, как надо показать текст — "
                + "волнистым, зачёркнутым или просто так, можно использовать HTML, чтобы им подсказывать.\n\n"
                + "Вот такие вот символы — &lt;i&gt;&lt;/i&gt; — называются тегами. Чтобы применить к тексту стиль, "
                + "оберни его в тег, вот так вот: &lt;i&gt;<i>волнистый текст</i>&lt;/i&gt;. Вот такие стили "
                + "позволяет использовать телеграм:\n\n"
                + "&lt;b&gt;<b>жирный</b>&lt;/b&gt;,\n"
                + "&lt;i&gt;<i>волнистый</i>&lt;/i&gt;,\n"
                + "&lt;u&gt;<u>подчёркнутый</u>&lt;/u&gt;,\n"
                + "&lt;s&gt;<s>зачёркнутый</s>&lt;/s&gt;,\n"
                + "&lt;tg-spoiler&gt;<tg-spoiler>спойлер</tg-spoiler>&lt;/tg-spoiler&gt;,\n"
                + "&lt;a href=\"http://www.example.com/\"&gt;"
                + "<a href=\"http://www.example.com/\">ссылка текстом</a>&lt;/a&gt;,\n"
                + "&lt;code&gt;<code>моноширинный текст</code>&lt;/code&gt;",
            ReplyMarkup = HtmlReferenceMenuMarkup.HtmlReferenceBack,
            ParseMode = ParseMode.Html
        };
}