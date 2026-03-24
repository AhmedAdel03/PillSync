using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PillSync.Data;
using PillSync.Data.Repo;
using PillSync.Services;
using PillSync.Services.Interface;
using TurboSMTP;

var builder = WebApplication.CreateBuilder(args);

// Ensure the app listens to the port Render provides
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IMemberRepo, MemberRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IOTP, OTPService>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Cannot get Token key");
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience=false
    };

});
builder.Services.AddSingleton<TurboSMTPClient>(sp =>
{
var turboSettings = builder.Configuration.GetSection("TurboSetting");
    var config= new TurboSMTPClientConfiguration.Builder()
                .SetConsumerKey(turboSettings["Consumer-Key"])
                .SetConsumerSecret(turboSettings["Consumer-Secret"])
                .SetServerURL(turboSettings["ServerUrl"])
                .SetSendServerURL(turboSettings["SendServerUrl"])
                .SetTimeZone("-03:00")
                .Build();
                return new TurboSMTPClient(config);
});


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

//Check for Render
app.MapGet("/", () => "PillSync API is running!");

app.Run();