global using fightAPI.Models; // This is the namespace for the Fighter class
global using fightAPI.Services.FighterService; 
global using fightAPI.Dtos.Fighter;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using fightAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Add EntityFrameworkCore.InMemory via NuGet

builder.Services.AddControllers();
// To use Swagger/OpenAPI goto http://localhost:5046/swagger/index.html
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme // this allows us to use the Authorize button in Swagger
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Add AutoMapper via NuGet
builder.Services.AddScoped<IFighterService, FighterService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

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

app.UseAuthentication(); // This must be before app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
