using Microsoft.EntityFrameworkCore;
using NattiDigestBot.Data.Models;
using NattiDigestBot.Data.Models.Configurations;

namespace NattiDigestBot.Data;

internal class DigestContext : DbContext
{
    public DigestContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DigestConfiguration).Assembly);
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Digest> Digests { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<DigestEntry> DigestEntries { get; set; }
}