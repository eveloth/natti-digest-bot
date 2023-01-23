namespace NattiDigestBot.Domain;

public class DigestEntry
{
    public int DigestEntryId { get; set; }
    public string Description { get; set; } = default!;
    public string MessageLink { get; set; } = default!;

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = default!;
    public long DigestId { get; set; }
    public DateOnly Date { get; set; }
    public virtual Digest Digest { get; set; } = default!;
}