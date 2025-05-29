

using AppCapasCitas.Infrastructure;

using AppCapasCitas.Application;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Logging;
using AppCapasCitas.Identity;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Infrastructure.Data;
using AppCapasCitas.Identity.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
string NombrePolitica = "WebApiConf";

builder.Services.AddControllers();
//CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy(NombrePolitica, config =>
    {
        config.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    }
    );
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));      

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
builder.Services.ConfigureIdentityServices(builder.Configuration);



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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = service.GetRequiredService<InfrastructureDbContext>();
        await context.Database.MigrateAsync();
        var contextIdentity = service.GetRequiredService<CleanArchitectureIdentityDbContext>();
        await contextIdentity.Database.MigrateAsync();
        // //insertar data
        // await CitasDbContextSeed.SeedAsync(context,loggerFactory);
        // await CitasDbContextSeedData.LoadDataAsync(context,loggerFactory);

        // //identity
        // var contextIdentity = service.GetRequiredService<CleanArchitectureIdentityDbContext>();
        // await contextIdentity.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error de migración");
    }
}
app.UseCors(NombrePolitica);
app.Run();
