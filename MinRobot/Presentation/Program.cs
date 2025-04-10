using MinRobot.Api.Endpoints;
using MinRobot.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MongoDBApi", policy => 
    {
        policy.WithOrigins("http://localhost:3000", "https://yourfrontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// register the concrete of PostgresSqlDbConnectionFactory ( could switch later and use SQL or SQLite i.e. just creata a new factory : IGenericDbConnectionFactory)
// Will this switch automatically? // Maybe put this in it's own extension
// needed to utilize appsettings
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Console.WriteLine($"[DEBUG] Postgres connection string: {builder.Configuration.GetConnectionString("PostgreSqlConnection")}");

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
builder.Services.AddApplicationServices(builder.Configuration, logger);

var app = builder.Build();

app.UseCors("MongoDBApi");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapRobotStatusEndpoints(); // use extensions here to add endpoints! enhances readability
app.MapRobotCommandEndpoints(); // use extensions here to add endpoints! enhances readability
app.MapRobotHistoryEndpoint(); // use extensions here to add endpoints! enhances readability
app.MapDbHealthEndpoint();
app.MapHealthCheckEndpoint();

app.Run();
