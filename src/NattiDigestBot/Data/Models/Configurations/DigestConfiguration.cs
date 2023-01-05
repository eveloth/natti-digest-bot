using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NattiDigestBot.Data.Models.Configurations;

public class DigestConfiguration : IEntityTypeConfiguration<Digest>
{
    public void Configure(EntityTypeBuilder<Digest> builder)
    {
        builder.HasKey(d => new {d.AccountId, d.Date});
    }
}