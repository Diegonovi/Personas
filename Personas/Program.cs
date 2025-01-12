using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PersonasDbContext> (options =>
     options.UseMySql(
         builder.Configuration.GetConnectionString("DefaultConnection"), 
         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
     )
    );

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPersonasService, PersonaService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();