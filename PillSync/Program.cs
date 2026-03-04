using Microsoft.EntityFrameworkCore;
using PillSync.Data;
using PillSync.Data.Repo;
using PillSync.Services;

var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddDbContext<AppDbContext>(option =>
  option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
 );

builder.Services.AddScoped<IMemberRepo,MemberRepo>();
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
