using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftFin.Api.Models;
using SoftFin.Core.Models;
using System.Reflection;

namespace SoftFin.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : 
    IdentityDbContext
    <
    User,
    IdentityRole<long>,
    long,
    IdentityUserClaim<long>,
    IdentityUserRole<long>,
    IdentityUserLogin<long>,
    IdentityRoleClaim<long>,
    IdentityUserToken<long>
        >(options)
{
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
