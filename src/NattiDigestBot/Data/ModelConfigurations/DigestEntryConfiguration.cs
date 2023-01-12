using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Data.ModelConfigurations;

public class DigestEntryConfiguration : IEntityTypeConfiguration<DigestEntry>
{
    public void Configure(EntityTypeBuilder<DigestEntry> builder)
    {
        builder
            .HasOne(d => d.Digest)
            .WithMany(d => d.DigestEntries)
            .HasForeignKey(d => new { d.DigestId, d.Date });

        builder
            .HasOne(d => d.Category)
            .WithMany(d => d.DigestEntries)
            .HasForeignKey(d => d.CategoryId);

        builder.HasKey(d => new { d.DigestId, d.Date });

        builder.Property(d => d.Description).IsRequired();
        builder.Property(d => d.MessageLink).IsRequired();
    }
}