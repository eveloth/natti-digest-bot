using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NattiDigestBot.Domain;

namespace NattiDigestBot.Data.ModelConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        //builder.HasIndex(x => x.GroupId).IsUnique();
    }
}