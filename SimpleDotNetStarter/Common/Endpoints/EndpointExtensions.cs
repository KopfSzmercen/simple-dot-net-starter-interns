namespace SimpleDotNetStarter.Common.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}