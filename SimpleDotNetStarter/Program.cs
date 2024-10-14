using SimpleDotNetStarter.Auth;
using SimpleDotNetStarter.Common.Endpoints;
using SimpleDotNetStarter.Persistence;
using SimpleDotNetStarter.Users.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

builder.Services.AddAuth();
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

app.AddUsersEndpoints();

await app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();