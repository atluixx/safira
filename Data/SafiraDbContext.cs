using Microsoft.EntityFrameworkCore;
using Safira.Data.Models;

namespace Safira.Data;

public class SafiraDbContext(DbContextOptions<SafiraDbContext> options) : DbContext(options)
{
    public DbSet<UserWallet> Wallets => Set<UserWallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserWallet>()
            .HasIndex(w => new { w.UserId, w.GuildId })
            .IsUnique();
    }
}
