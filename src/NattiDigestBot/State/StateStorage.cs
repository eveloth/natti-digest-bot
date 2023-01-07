namespace NattiDigestBot.State;

public static class StateStorage
{
    public static Dictionary<long, ChatMode> ChatMode { get; set; } = new();
    public static Dictionary<long, long> WaitingForConfirmation { get; set; } = new();
    public static Dictionary<long, DateOnly> CurrentDigest { get; set; } = new();
}