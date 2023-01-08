namespace NattiDigestBot.State;

public static class StateStorage
{
    private static string _botName;

    public static string? BotName
    {
        get => _botName;
        set =>
            _botName =
                value ?? throw new NullReferenceException("Couldn't get bot username, exiting");
    }

    public static Dictionary<long, ChatMode> ChatMode { get; set; } = new();
    public static Dictionary<long, long> WaitingForConfirmation { get; set; } = new();
    public static Dictionary<long, DateOnly> CurrentDigest { get; set; } = new();

    public static ChatMode GetChatMode(long userId)
    {
        if (!ChatMode.ContainsKey(userId))
        {
            ChatMode.Add(userId, State.ChatMode.Normal);
        }

        return ChatMode[userId];
    }

    public static void SetChatModeFor(long userId, ChatMode chatMode)
    {
        if (!ChatMode.ContainsKey(userId))
        {
            ChatMode.Add(userId, chatMode);
        }

        ChatMode[userId] = chatMode;
    }

    public static void AddToWaitingForConfirmationList(long groupId, long userId)
    {
        ChatMode[userId] = State.ChatMode.WaitingForConfirmation;
        WaitingForConfirmation.Add(groupId, userId);
    }

    public static void RemoveFromWaitingForConfirmationList(long groupId)
    {
        WaitingForConfirmation.Remove(groupId);
    }

    public static bool IsWaitingForConfirmation(long groupId)
    {
        return WaitingForConfirmation.ContainsKey(groupId);
    }

    public static bool IsConfirmationRequestedForUser(long userId)
    {
        return WaitingForConfirmation.ContainsValue(userId);
    }

    public static long GetGroupOwner(long groupId)
    {
        return WaitingForConfirmation[groupId];
    }

    public static DateOnly GetCurrentDigestDate(long userId)
    {
        return CurrentDigest[userId];
    }

    public static void SetCurrentDigest(long userId, DateOnly digestDate)
    {
        if (!CurrentDigest.ContainsKey(userId))
        {
            CurrentDigest.Add(userId, digestDate);
        }
        else
        {
            CurrentDigest[userId] = digestDate;
        }
    }
}