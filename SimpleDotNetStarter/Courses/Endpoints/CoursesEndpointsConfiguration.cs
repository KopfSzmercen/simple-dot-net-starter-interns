using SimpleDotNetStarter.Common.Endpoints;

namespace SimpleDotNetStarter.Courses.Endpoints;

internal static class CoursesEndpointsConfiguration
{
    public const string BaseUrl = "courses";
    public const string GroupTag = "Courses";

    public static void AddCoursesEndpoints(this WebApplication webApplication)
    {
        var group = webApplication
            .MapGroup(BaseUrl)
            .WithTags(GroupTag);

        group
            .MapEndpoint<CreateCourseEndpoint>()
            .MapEndpoint<BrowseCoursesEndpoint>();
    }
}