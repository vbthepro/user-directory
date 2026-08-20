using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserDirectory.Application;
using UserDirectory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=/data/app.db"));
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Directory API", Version = "v1" }));
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var authEnabled = builder.Configuration.GetValue<bool>("Authentication:Enabled");
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

var users = app.MapGroup("/api/users");
if (authEnabled)
{
    users.RequireAuthorization();
}

users.MapGet("/", async (UserService service, CancellationToken ct) => Results.Ok(await service.ListAsync(ct)));
users.MapGet("/{id:guid}", async (Guid id, UserService service, CancellationToken ct) =>
{
    var user = await service.GetAsync(id, ct);
    return user is null ? Results.NotFound() : Results.Ok(user);
});
users.MapPost("/", async (UserRequest request, UserService service, CancellationToken ct) =>
{
    try { return Results.Created("", await service.CreateAsync(request, ct)); }
    catch (System.ComponentModel.DataAnnotations.ValidationException ex) { return Results.ValidationProblem(new Dictionary<string, string[]> { ["request"] = [ex.Message] }); }
});
users.MapPut("/{id:guid}", async (Guid id, UserRequest request, UserService service, CancellationToken ct) =>
{
    try
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return updated is null ? Results.NotFound() : Results.Ok(updated);
    }
    catch (System.ComponentModel.DataAnnotations.ValidationException ex) { return Results.ValidationProblem(new Dictionary<string, string[]> { ["request"] = [ex.Message] }); }
});
users.MapDelete("/{id:guid}", async (Guid id, UserService service, CancellationToken ct) =>
    await service.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound());

app.Run();

public partial class Program { }
