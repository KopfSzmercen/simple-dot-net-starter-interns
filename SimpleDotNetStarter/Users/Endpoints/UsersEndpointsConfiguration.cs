using SimpleDotNetStarter.Common.Endpoints;

namespace SimpleDotNetStarter.Users.Endpoints;

internal static class UsersEndpointsConfiguration
{
    public const string BaseUrl = "users";
    public const string GroupTag = "users";

    public static void AddUsersEndpoints(this WebApplication webApplication)
    {
        var group = webApplication
            .MapGroup(BaseUrl)
            .WithTags(GroupTag);

        group
            .MapEndpoint<RegisterUserEndpoint>()
            .MapEndpoint<SignInUserJwtEndpoint>()
            .MapEndpoint<GetMeEndpoint>();
    }
}