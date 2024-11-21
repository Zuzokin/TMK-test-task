using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PipeManager.Application.Auth;
using PipeManager.Application.Mapping;
using PipeManager.Application.Services;
using PipeManager.Core.Abstractions;
using PipeManager.DataAccess;
using PipeManager.DataAccess.Repositories;
using PipeManager.Infrastructure;
using PipeManager.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PipeManagerDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PipeManagerDbContext)));
    });

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddScoped<IPipesService, PipesService>();
builder.Services.AddScoped<ISteelGradesService, SteelGradesService>();
builder.Services.AddScoped<IPackagesService, PackagesService>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IPipesRepository, PipesRepository>();
builder.Services.AddScoped<ISteelGradesRepository, SteelGradesRepository>();
builder.Services.AddScoped<IPackagesRepository, PackagesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",   
                    "http://frontend:3000"      
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


var app = builder.Build();

// Apply migrations on application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<PipeManagerDbContext>();
        context.Database.Migrate(); // Applies pending migrations
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
        throw; // Re-throw the exception to prevent application startup
    }
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// Middleware for handling exceptions
app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception != null)
        {
            var statusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                Error = exception.Message,
                StatusCode = statusCode
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

// Middleware for logging HTTP requests and responses
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
