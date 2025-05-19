

using AppCapasCitas.Infrastructure;
using AppCapasCitas.Application;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Logging;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Configurar logging para HTML
string _baseLogPath = "Logs/logs"; // Sin extensión para permitir la rotación diaria
var fullPath = Path.Combine(Directory.GetCurrentDirectory(), _baseLogPath + ".html");

// Crear directorio si no existe
Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
builder.Logging.AddProvider(new HtmlFileLoggerProvider(fullPath));

// Registrar LoggerAdapter como servicio
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));


builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AppCapasCitas API",
        Version = "v1",
        Description = "API for the AppCapasCitas application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ernesto De la O",
            Email = "erviquez@gmail.com"
        }
    });      
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppCapasCitas API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
    app.MapOpenApi();
}

//app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();
app.Run();
