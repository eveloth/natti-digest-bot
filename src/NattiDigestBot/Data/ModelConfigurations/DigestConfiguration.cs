using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Data.ModelConfigurations;

public class DigestConfiguration : IEntityTypeConfiguration<Digest>
{
    public void Configure(EntityTypeBuilder<Digest> builder)
    {
        builder.HasKey(d => new {d.AccountId, d.Date});
    }
}