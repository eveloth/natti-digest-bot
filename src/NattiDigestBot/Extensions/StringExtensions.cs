namespace NattiDigestBot.Extensions;

public static class StringExtensions
{
    public static string EscpapeHtmlTagClosures(this string str)
    {
        return str.Replace("<", "&lt;").Replace(">", "&gt;");
    }
}