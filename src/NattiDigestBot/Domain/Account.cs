namespace NattiDigestBot.Domain;

public class Account
{
    public long AccountId { get; set; }
    public long? GroupId { get; set; }
    public bool IsGroupConfirmed { get; set; }
    public virtual List<Category> Categories { get; set; } = default!;
    public virtual List<Digest> Digests { get; set; } = default!;
    public int? PinnedDigestMessageId { get; set; }
}