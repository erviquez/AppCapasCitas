using AppCapasCitas.Infrastructure;
using AppCapasCitas.Application;
using AppCapasCitas.Reporting;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Logging;
using AppCapasCitas.Identity;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Infrastructure.Data;
using AppCapasCitas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Infrastructure.Repositories.Messaging;
using AppCapasCitas.Infrastructure.Repositories.Shortner;
using AppCapasCitas.DTO.Configuration;
var builder = WebApplication.CreateBuilder(args);
string NombrePolitica = "WebApiConf";

builder.Services.AddControllers();
//CORS 
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
// Agregar servicio de reportes
builder.Services.AddReportingServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy(NombrePolitica, config =>
    {
        config
            //.WithOrigins(allowedOrigins!)
            .AllowAnyOrigin()
            //.AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }
    );
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("SmsSettings"));
builder.Services.Configure<ShortnerSettings>(builder.Configuration.GetSection("ShortnerSettings"));
builder.Services.Configure<UrlsConfirmationSettings>(builder.Configuration.GetSection("UrlsConfirmationSettings"));
//builder.Services.Configure<ReporteConfiguration>(builder.Configuration.GetSection("ReporteSettings"));
builder.Services.AddHttpClient<ISmsService, SmsService>();
builder.Services.AddHttpClient<IShortnerService, ShortnerService>();
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
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    // Configurar autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese 'Bearer' seguido de un espacio y el token JWT.\n\nEjemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Incluir comentarios XML (opcional)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
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

        // Configuraciones adicionales para mejor experiencia
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DefaultModelsExpandDepth(-1); // Ocultar modelos por defecto
        c.DisplayRequestDuration();
        c.EnableValidator();
        c.ShowExtensions();
        
        // Personalización del tema
        c.InjectStylesheet("/swagger-ui/custom.css");
    });
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseStaticFiles(); 
app.UseCors(NombrePolitica);
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
app.Run();
