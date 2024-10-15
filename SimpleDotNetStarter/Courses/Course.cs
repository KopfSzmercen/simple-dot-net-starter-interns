using SimpleDotNetStarter.Users;

namespace SimpleDotNetStarter.Courses;

internal sealed class Course
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public Guid CreatorId { get; init; }

    public User Creator { get; init; }
}