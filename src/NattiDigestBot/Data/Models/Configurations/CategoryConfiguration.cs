using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NattiDigestBot.Data.Models.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.AccountId).IsRequired();
        builder.Property(c => c.Keyword).IsRequired();
        builder.Property(c => c.Description).IsRequired();
    }
}