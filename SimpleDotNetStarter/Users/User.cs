using Microsoft.AspNetCore.Identity;
using SimpleDotNetStarter.Courses;

namespace SimpleDotNetStarter.Users;

internal sealed class User : IdentityUser<Guid>
{
    public List<Course> CreatedCourses { get; set; } = null!;
}