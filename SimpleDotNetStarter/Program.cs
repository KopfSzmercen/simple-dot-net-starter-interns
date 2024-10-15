using SimpleDotNetStarter.Auth;
using SimpleDotNetStarter.Common.Cors;
using SimpleDotNetStarter.Common.Endpoints;
using SimpleDotNetStarter.Courses.Endpoints;
using SimpleDotNetStarter.Persistence;
using SimpleDotNetStarter.Users.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

builder.Services.AddAuth();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);

var app = builder.Build();

app.AddUsersEndpoints();
app.AddCoursesEndpoints();

await app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.Run();