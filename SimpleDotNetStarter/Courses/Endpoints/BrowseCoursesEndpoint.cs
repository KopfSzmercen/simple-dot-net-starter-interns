using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleDotNetStarter.Common.Endpoints;
using SimpleDotNetStarter.Persistence;

namespace SimpleDotNetStarter.Courses.Endpoints;

public class BrowseCoursesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("", Handle)
            .WithSummary("Browse all courses");
    }

    private static async Task<IEnumerable<CourseResponse>> Handle(
        [FromServices] AppDbContext dbContext
    )
    {
        var courses = await dbContext.Courses
            .Include(x => x.Creator)
            .Select(x => new CourseResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Creator = new CourseCreator(x.Creator.Id, x.Creator.Email!)
            })
            .ToListAsync();

        return courses;
    }

    public sealed record CourseCreator(
        Guid Id,
        string Email
    );

    public sealed record CourseResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string? Description { get; init; }

        public CourseCreator Creator { get; init; }
    }
}