using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleDotNetStarter.Auth.Tokens;
using SimpleDotNetStarter.Common.Endpoints;

namespace SimpleDotNetStarter.Users.Endpoints;

internal sealed class SignInUserJwtEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("sign-in-jwt", Handle)
            .WithSummary("Sign in a user and return a JWT token");
    }

    private static async Task<Results<Ok<SignInUserJwtResponse>, BadRequest<string>>> Handle(
        [FromBody] SignInUserJwtRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] ITokensManager jwtTokenService
    )
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return TypedResults.BadRequest("User not found");

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!signInResult.Succeeded)
            return TypedResults.BadRequest("Invalid password");

        var token = jwtTokenService.CreateToken(
            user.Id,
            []
        );

        return TypedResults.Ok(
            new SignInUserJwtResponse { Token = token.AccessToken }
        );
    }

    internal sealed record SignInUserJwtRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }

    public sealed record SignInUserJwtResponse
    {
        public required string Token { get; init; }
    }
}