using Microsoft.EntityFrameworkCore;
using SoftFin.Core.Models;

namespace SoftFin.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration( );
    }
}
