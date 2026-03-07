using Microsoft.EntityFrameworkCore;
using PillSync.Data;
using PillSync.Data.Repo;
using PillSync.Services;

var builder = WebApplication.CreateBuilder(args);

// Ensure the app listens to the port Render provides
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IMemberRepo, MemberRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Only use HTTPS redirection in development or if specifically required.
// Render handles SSL termination for you.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

// Simple Health Check for Render
app.MapGet("/", () => "PillSync API is running!");

app.Run();