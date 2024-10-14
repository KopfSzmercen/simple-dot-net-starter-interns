using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleDotNetStarter.Users;

namespace SimpleDotNetStarter.Persistence;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("users");
    }
}