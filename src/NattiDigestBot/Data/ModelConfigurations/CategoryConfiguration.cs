using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Data.ModelConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.AccountId).IsRequired();
        builder.Property(c => c.Keyword).IsRequired();
        builder.HasIndex(c => c.Keyword).IsUnique();
        builder.Property(c => c.Description).IsRequired();
    }
}