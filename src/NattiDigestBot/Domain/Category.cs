namespace NattiDigestBot.Domain;

public class Category
{
    public int CategoryId { get; set; }
    public long AccountId { get; set; }
    public string Keyword { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int DisplayOrder { get; set; }
    public virtual List<DigestEntry> DigestEntries { get; set; } = default!;
}