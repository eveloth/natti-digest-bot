namespace NattiDigestBot.Domain;

public class Digest
{
    public long AccountId { get; set; }
    public DateOnly Date { get; set; }
    public string? DigestText { get; set; }
    public bool IsSent { get; set; }
    public virtual List<DigestEntry> DigestEntries { get; set; } = default!;
}