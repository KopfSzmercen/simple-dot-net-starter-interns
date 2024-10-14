using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleDotNetStarter.Common.Endpoints;
using SimpleDotNetStarter.Persistence;

namespace SimpleDotNetStarter.Users.Endpoints;

internal sealed class GetMeEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("me", Handle)
            .RequireAuthorization()
            .WithSummary("Get the current user");
    }

    private static async Task<Results<
            Ok<GetMeResponse>,
            BadRequest<string>
        >
    > Handle(
        [FromServices] AppDbContext dbContext,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken
    )
    {
        var userId = httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return TypedResults.BadRequest("User id not found");

        var currentUser = await dbContext.Users
            .Where(x => x.Id == Guid.Parse(userId))
            .FirstOrDefaultAsync(cancellationToken);

        if (currentUser is null)
            return TypedResults.BadRequest("User not found");

        return TypedResults.Ok(
            new GetMeResponse
            {
                Id = currentUser.Id,
                Email = currentUser.NormalizedEmail!
            }
        );
    }

    internal sealed record GetMeResponse
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
    }
}