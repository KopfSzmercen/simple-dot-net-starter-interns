namespace SimpleDotNetStarter.Common.Cors;

internal static class CorsExtensions
{
    private const string CorsPolicy = "CorsPolicy";

    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy,
                builder => builder
                    .WithOrigins(corsOptions!.AllowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        return services;
    }

    public static void UseCors(this WebApplication app)
    {
        app.UseCors(CorsPolicy);
    }
}