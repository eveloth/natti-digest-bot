using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NattiDigestBot.Data.Models.Configurations;

public class DigestEntryConfiguration : IEntityTypeConfiguration<DigestEntry>
{
    public void Configure(EntityTypeBuilder<DigestEntry> builder)
    {
        builder
            .HasOne(d => d.Digest)
            .WithMany(d => d.DigestEntries)
            .HasForeignKey(d => new { d.DigestId, d.Date });

        builder.HasKey(d => new {d.DigestId, d.Date});

        builder.Property(d => d.Description).IsRequired();
        builder.Property(d => d.MessageLink).IsRequired();
    }
}