using MinRobot.Application.Endpoints;
using MinRobot.Application;
using MinRobot.Infrastructure.Factories;
using MinRobot.Domain.Interfaces;
using MinRobot.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinRobot.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// register the concrete of PostgresSqlDbConnectionFactory ( could switch later and use SQL or SQLite i.e. just creata a new factory : IGenericDbConnectionFactory)
// Will this switch automatically? // Maybe put this in it's own extension
// needed to utilize appsettings
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapRobotStatusEndpoints(); // use extensions here to add endpoints! enhances readability

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
