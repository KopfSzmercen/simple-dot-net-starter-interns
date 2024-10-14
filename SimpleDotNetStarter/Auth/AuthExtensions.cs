﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleDotNetStarter.Auth.Tokens;
using SimpleDotNetStarter.Persistence;
using SimpleDotNetStarter.Users;

namespace SimpleDotNetStarter.Auth;

internal static class AuthExtensions
{
    public static void AddAuth(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtTokensOptionsSetup>();

        services.AddSingleton<ITokensManager, TokensManager>();

        services.AddAuthorization();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;
        });

        services
            .AddAuthentication(o =>
            {
                o.DefaultScheme = AuthorizationSchemes.SmartScheme;
                o.DefaultChallengeScheme = AuthorizationSchemes.SmartScheme;
            })
            .AddPolicyScheme(AuthorizationSchemes.SmartScheme, "Bearer or Cookies", o =>
            {
                o.ForwardDefaultSelector = context =>
                {
                    var authHeader = context.Request.Headers.Authorization;

                    var isJwtBearerVariant = !string.IsNullOrEmpty(authHeader) &&
                                             authHeader.ToString().StartsWith(JwtBearerDefaults.AuthenticationScheme);

                    return isJwtBearerVariant
                        ? JwtBearerDefaults.AuthenticationScheme
                        : IdentityConstants.ApplicationScheme;
                };
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var provider = services.BuildServiceProvider();

                var jwtOptions = provider.GetRequiredService<IOptions<JwtTokensOptions>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SigningKey)),
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer
                };
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Cookie.Name = "SimpleDotNet.Users";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToLogout = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

        services
            .AddIdentityCore<User>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();

        services.AddDataProtection()
            .PersistKeysToDbContext<AppDbContext>();
    }
}