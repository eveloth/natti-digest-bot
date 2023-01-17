using Telegram.Bot.Types.Enums;

namespace NattiDigestBot.Replies.Menus;

public static class HelpMenu
{
    public static Reply Entrypoint { get; } =
        new()
        {
            ReplyText =
                "Для того, чтобы начать пользоваться ботом, тебе сперва нужно добавить бота в группу, "
                + "в которую ты хочешь отправлять дайджесты, привязать её, "
                + "а затем подтвердить, что ты — её администратор.\n"
                + "Это нужно для того, чтобы кто угодно не мог привязать твою или любую другую группу "
                + "и использовать этот бот для рассылки спама.\n"
                + "Бот также должен быть админитратором группы и иметь возможность удалять сообщения — "
                + "другие разрешения ему не нужны.\n\n"
                + "После этого тебе нужно создать категории, на которые будет разбит твой дайджест — хотя бы одну, "
                + "и всё, можешь составлять первый дайджест! Он будет выглядеть примерно так:\n\n"
                + "<b>Дайджест от 01/01/97</b>\n\nВот, что интересного было сегодня!\n\n"
                + "<b>Полезные сайты, которыми вы поделились:</b>\n"
                + "\u2022 Нейросеть делает тестовые задания (и плохо справляется) - <i>ссылка</i>\n"
                + "\u2022 А это сайт о том, как дрессировать котиков - <i>ссылка</i>\n"
                + "<b>Просьбы о помощи:</b>\n"
                + "\u2022 Нужна помощь с перевозкой шкафа из Магадана в Лиссабон - <i>ссылка</i>\n\n"
                + "Если ты планируешь добавить бота в приватную группу, обязательно посмотри информацию о них "
                + "в меню.\n\n"
                + "Ниже ты найдёшь инструкции, которые быстро помогут научиться тебе пользоваться ботом:",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.EntrypointMarkup
        };

    public static Reply GroupIdSection { get; } =
        new()
        {
            ReplyText =
                "Чтобы узнать ID своей группы, открой eё в веб-версии Telegram "
                + "(<a href=\"https://web.telegram.org/z/\">тык</a>) "
                + "и посмотри в адресную строку, она должна выглядеть примерно так: \n\n"
                + "<i>https://web.telegram.org/z/-987654321</i>\n\n"
                + "Нам нужно число, которое идёт после / и начинается с минуса.\n"
                + "Теперь тебе нужно вставить <b>100</b> между минусом и числом:\n\n"
                + "было: -987654321\n"
                + "стало: -<b>100</b>987654321\n\n"
                + "Получилось? Теперь, используя этот ID, ты можешь привязать группу!",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.GroupIdBack
        };

    public static Reply BindSection { get; } =
        new()
        {
            ReplyText =
                "Чтобы привязать группу, узнай её ID (о том, как это сделать, ты можешь узнать, нажав на кнопку ниже "
                + "или в главном меню), и введи следующую команду:\n\n"
                + "/bind <i>id_группы</i>\n\n"
                + "например:\n/bind -100987654321\n\n"
                + "После этого тебе нужно будет её подтвердить — "
                + "когда привяжешь её, появится инструкция по подтверждению. "
                + "Чтобы отвязать группу, просто введи /unbind.",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.BindBack
        };

    public static Reply CategorySection { get; } =
        new()
        {
            ReplyText =
                "Каждый дайджест разделяется на категории, например, \"полезные сайты\" или \"просьбы о помощи\". "
                + "У категории есть ключевое слово и описание: \"сайты - "
                + "Полезные сайты, которыми вы поделились\".\n\n"
                + "С помощью ключевого слова ты сможешь добавить сслыку на сообщение из чата "
                + "в определённую категорию, а когда ты дашь команду сформировать дайджест, "
                + "ключевое слово заменится развёрнутым описанием.\n\n"
                + "Чтобы добавить категорию, введи следующую команду:\n"
                + "/new_category <i>ключ - описание</i>\n\n"
                + "например:\n/new_category котики - Фотографии ваших котиков\n\n"
                + "Чтобы посмотреть все добавленные категории, введи команду /categories.\n"
                + "Рядом с категорией в скобках будет указан её ID. С помощью него можно "
                + "удалить категорию командой:\n/delete_category <i>111</i>\nили изменить её командой\n"
                + "/update_category <i>111 - коооотики - очень милые КОТИКИ</i>.\n\n"
                + "Помни, что если ты удалишь категорию, все связанные с ней сообщения тоже удалятся из дайджеста.",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.Back
        };

    public static Reply DigestSection { get; } =
        new()
        {
            ReplyText =
                "Чтобы создать дайджест, введи команду /digest <i>01/01/97</i>, "
                + "подставив вместо этой даты дату дайджеста. Если дайджест за эту дату уже был создан, "
                + "то он откроется для редактирования, а если нет, создастся пустой. "
                + "И в том, и в другом случае делать нужно одно и то же, сейчас расскажу.\n\n"
                + "После того, как ты введёшь эту команду, ты попадёшь в интерактиный режим. "
                + "Каждое твоё сообщение, за исключением четырёх команд, бот будет считать описанием "
                + "нового элемента дайджеста. Ты можешь пересылать ему сообщения из группы с такой подписью:\n"
                + "<i>категория (ключевое слово)</i>\n"
                + "<i>что интересного ждёт по ссылке</i>\n"
                + "<i>ссылка на сообщение</i>\n\n"
                + "Например:\n"
                + "<i>котики</i>\n"
                + "<i>очень красивый белый котик!</i>\n"
                + "<i>ссылка на сообщение</i>\n\n"
                + "Категорию, описание и сслыку нужно указывать с новой строки. "
                + "Пересылаемое сообщение бот проигнорирует, оно нужно только если тебе удобно "
                + "работать с ботом именно так.\n\n"
                + "Чтобы посмотреть, что уже есть в дайджесте, дай команду /raw_preview. "
                + "Бот пришлёт тебе список добавленных сообщений, рядом с каждым из которых будет указан его ID. "
                + "Похоже на работу с категориями, правда? Если ты передумаешь добавлять что-то в дайджест, "
                + "дай команду /delete <i>111</i>, подставив вместо 111 число, "
                + "которое стоит рядом с сообщением в дайджесте.\n\n"
                + "Если вдруг забудешь, какие у тебя есть категории, дай команду /categories. "
                + "Когда закончишь, дай команду /make, дайджест сформируется, и выйди из интерактивного режима "
                + "с помощью команды /exit.\n\n"
                + "Чтобы отредактировать готовый текст дайджеста, введи команду /edit <i>01/01/97</i>, "
                + "скопируй текст, который пришлёт тебе бот, отредактируй, если хочешь, "
                + "добавь стили с помощью HTML-разметки "
                + "и отправь дайджест в группу командой /send <i>01/01/97</i>. Отправить повторно "
                + "один и тот же дайджест будет нельзя.\n\n"
                + "Помни, что с того момента, как ты сформируешь дайджест командой /make, "
                + "то всегда сможешь посмотреть его текст с помощью команды /preview <i>01/01/97</i>.",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.Back
        };

    public static Reply AccountIdSection { get; } =
        new()
        {
            ReplyText = "DYNAMIC_VALUE_PLACEHOLDER",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.Back
        };

    public static Reply DeleteAccountSection { get; } =
        new()
        {
            ReplyText =
                "Если ты действительно хочешь удалить аккаунт, нажми на кнопку \"удалить\".\n\n"
                + "<b>Внимание: вся информация, связанная с твоим аккаунтом, включая привязанную группу, "
                + "категории и дайджесты, будет удалена, и ты не сможешь её восстановить!</b>",
            ParseMode = ParseMode.Html,
            ReplyMarkup = HelpMenuMarkups.DeleteAccountBack
        };

    public static Reply PrivateGroupsInfoSection { get; } =
        new()
        {
            ReplyText =
                "Отправлять дайджесты в приватные группы, то есть те, у которых нет публичной ссылки, "
                + "к сожалению, не получится. Это связано с особенностями работы Telegram — дело в том, "
                + "что у приватных групп их ID, уникальный идентификатор, с помощью которого бот "
                + "узнает вас и группу, периодически меняется. Можно, конечно, каждый раз смотреть, "
                + "не изменился ли ID группы и каждый раз привязывать и подтвреждать её заново, "
                + "но это звучит очень неудобно, правда?\n\n"
                + "Помимо этого к ID приватной группы не нужно приписывать 100, чтобы получить её "
                + "настоящий ID.\n\n"
                + "Тем не менее, такая возможность есть, просто нужно помнить о её недостатках.",
            ReplyMarkup = HelpMenuMarkups.Back
        };
}