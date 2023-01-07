namespace NattiDigestBot.Domain;

public class DigestEntry
{
    public int DigestEntryId { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public string MessageLink { get; set; }

    public long DigestId { get; set; }
    public DateOnly Date { get; set; }
    public virtual Digest Digest { get; set; }
}