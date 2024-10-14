﻿using Microsoft.EntityFrameworkCore;

namespace SimpleDotNetStarter.Persistence;

internal static class PersistenceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
    }

    public static async Task ApplyMigrations(this WebApplication app)
    {
        //apply migrations
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}