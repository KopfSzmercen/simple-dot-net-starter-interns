using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SimpleDotNetStarter.Common.Endpoints;
using SimpleDotNetStarter.Persistence;

namespace SimpleDotNetStarter.Courses.Endpoints;

public class CreateCourseEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("", Handle)
            .RequireAuthorization()
            .WithSummary("Create a new course");
    }

    private static async Task<CourseCreatedResponse> Handle(
        [FromBody] CreateCourseRequest request,
        [FromServices] AppDbContext dbContext,
        [FromServices] IHttpContextAccessor httpContextAccessor
    )
    {
        var userId = httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier)!;

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatorId = Guid.Parse(userId)
        };

        await dbContext.Courses.AddAsync(course);

        await dbContext.SaveChangesAsync();

        return new CourseCreatedResponse(course.Id);
    }

    public sealed record CreateCourseRequest
    {
        public required string Name { get; init; }

        public string? Description { get; init; }
    }

    public sealed record CourseCreatedResponse(Guid Id);
}