using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserDirectory.Application;
using UserDirectory.Application.Interfaces;
using UserDirectory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var authEnabled = builder.Configuration.GetValue<bool>("Authentication:Enabled");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=/data/app.db"));
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers(options =>
{
    if (authEnabled)
    {
        options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
    }
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Directory API", Version = "v1" }));
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

if (authEnabled)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Authentication:Authority"];
            options.Audience = builder.Configuration["Authentication:Audience"];
            options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        });
    builder.Services.AddAuthorization();
}

var app = builder.Build();

Directory.CreateDirectory("/data");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
if (authEnabled) { app.UseAuthentication(); app.UseAuthorization(); }
app.MapControllers();

app.Run();

public partial class Program { }
