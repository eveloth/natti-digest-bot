namespace NattiDigestBot.Replies.Menus;

public static class CallbackData
{
    #region Areas

    public static string Main { get; } = nameof(Main).ToLower();

    #endregion

    #region Actions

    public static string GroupId { get; set; } = nameof(GroupId).ToLower();
    public static string Bind { get; set; } = nameof(Bind).ToLower();
    public static string Categories { get; set; } = nameof(Categories).ToLower();
    public static string Digest { get; set; } = nameof(Digest).ToLower();
    public static string AccountId { get; set; } = nameof(AccountId).ToLower();
    public static string DeleteAccountPropmt { get; set; } = nameof(DeleteAccountPropmt).ToLower();
    public static string DeleteAccountConfirm { get; set; } = nameof(DeleteAccountConfirm).ToLower();
    public static string Back { get; set; } = nameof(Back).ToLower();

    #endregion
}