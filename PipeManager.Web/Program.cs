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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PipeManagerDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PipeManagerDbContext)));
        // options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PipeManagerDbContext)));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
