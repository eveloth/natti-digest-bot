namespace NattiDigestBot.Domain;

public class Account
{
    public long AccountId { get; set; }
    public long? GroupId { get; set; }
    public bool IsGroupConfirmed { get; set; }
    public List<Category> Categories { get; set; }
    public List<Digest> Digests { get; set; }
}