using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleDotNetStarter.Auth;
using SimpleDotNetStarter.Common.Endpoints;

namespace SimpleDotNetStarter.Users.Endpoints;

public class RegisterUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("", Handle)
            .WithSummary("Register a new user");
    }

    private static async Task<Results<NoContent, BadRequest<string>>> Handle(
        [FromBody] RegisterUserRequest request,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            Id = Guid.NewGuid()
        };

        var createUserResult = await userManager.CreateAsync(user, request.Password);

        if (!createUserResult.Succeeded)
            return TypedResults.BadRequest(
                string.Join(", ", createUserResult.Errors.Select(e => e.Description)
                )
            );

        await userManager.AddClaimsAsync(user, [new Claim(CustomClaims.Id, user.Id.ToString())]);

        return TypedResults.NoContent();
    }

    internal sealed record RegisterUserRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}