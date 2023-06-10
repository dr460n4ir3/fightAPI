global using fightAPI.Models; // This is the namespace for the Fighter class
global using fightAPI.Services.FighterService; 
global using fightAPI.Dtos.Fighter;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using fightAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Add EntityFrameworkCore.InMemory via NuGet

builder.Services.AddControllers();
// To use Swagger/OpenAPI goto http://localhost:5046/swagger/index.html
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Add AutoMapper via NuGet
builder.Services.AddScoped<IFighterService, FighterService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation"); // Specify the route for the Swagger JSON document
        c.RoutePrefix = "swagger"; // Specify the base route for the Swagger UI
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
