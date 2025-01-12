using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Personas.Database;
using Personas.Repository;
using Personas.Service;
using Serilog;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"logger.{environment}.json", optional: false, reloadOnChange: true)
    .Build();

var builder = WebApplication.CreateBuilder(args);

SetUpLogger(configuration);
SetUpSwagger();

builder.Services.AddControllers();

builder.Services.AddDbContext<PersonasDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

builder.Services.AddScoped<IPersonasService, PersonaService>();
builder.Services.AddScoped<IPersonasRepository, PersonasRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void SetUpLogger(IConfigurationRoot configuration)
{
    Console.OutputEncoding = Encoding.UTF8;

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    builder.Services.AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSerilog(logger, true);
    });
}

void SetUpSwagger()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        // Otros metadatos de la API
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Personas API",
            Description = "A test API"
        });
    });
}