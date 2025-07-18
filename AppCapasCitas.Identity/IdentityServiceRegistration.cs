using System.Text;
using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Identity.Data;
using AppCapasCitas.Identity.Models;
using AppCapasCitas.Identity.Policies;
using AppCapasCitas.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AppCapasCitas.Identity;

public static class IdentityServiceRegistration
{
    /// <summary>
    /// Configura y registra todos los servicios relacionados con identidad para la inyección de dependencias
    /// </summary>
    /// <param name="services">La colección IServiceCollection para agregar servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>La colección IServiceCollection configurada</returns>
    public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configura las opciones de JWT desde la sección "JwtSettings" del archivo de configuración
        var sectionJwtSettings = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(sectionJwtSettings);
        // services.Configure<EmailSettings>( c => configuration.GetSection("EmailSettings"));

        // Configura el DbContext de Entity Framework para Identity con SQL Server
        // Especifica la cadena de conexión y el ensamblado donde están las migraciones
        services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
            b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)));

        // Configura ASP.NET Core Identity con ApplicationUser personalizado y IdentityRole
        // Esto establece la gestión de usuarios, roles y proveedores de tokens por defecto
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<CleanArchitectureIdentityDbContext>()
                .AddDefaultTokenProviders();

        // Configura políticas de autorización para control de acceso basado en roles y claims
        services.AddAuthorization(options =>
        {
            // Políticas basadas en roles
            options.AddPolicy("RequireAdminRole", policy => 
                policy.RequireRole("Administrador"));
            options.AddPolicy("RequireMedicoRole", policy => 
                policy.RequireRole("Medico"));
            options.AddPolicy("RequireOperadorRole", policy => 
                policy.RequireRole("Operador"));
            options.AddPolicy("RequirePacienteRole", policy => 
                policy.RequireRole("Paciente"));
            options.AddPolicy("RequireRegistradoRole", policy => 
                policy.RequireRole("Registrado"));
                
            // Política basada en claims - verifica si el email está confirmado
            options.AddPolicy("RequireEmailVerified", policy => 
                policy.RequireClaim("EmailVerified", "true"));
                
            // Política personalizada - verifica edad mínima de 18 años
            options.AddPolicy("Over18YearsOld", policy =>
                policy.Requirements.Add(new MinimumAgeRequirement(18)));
        });

        // Registra el manejador para la política personalizada de edad mínima
        services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        
        // Registra el servicio de autenticación para su uso en la aplicación
        services.AddTransient<IAuthService, AuthService>();

        // Configuración de parámetros para validación de tokens JWT
        var jwtSettings = sectionJwtSettings.Get<JwtSettings>();
        var key = Encoding.UTF8.GetBytes(jwtSettings!.Key);

        // Verificar que la clave tenga la longitud mínima
        if (key.Length < 32)
        {
            throw new ArgumentException("La clave JWT debe tener al menos 32 caracteres (256 bits)");
        }
        // Configuración de parámetros para validación de tokens JWT
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Valida el emisor (issuer)
            ValidIssuer = jwtSettings.Issuer,
            //ValidIssuer =  sectionJwtSettings["Issuer"], 
            ValidateIssuerSigningKey = true, // Valida la firma del emisor
            ValidateAudience = true, // Valida la audiencia (audience)
            ValidAudience = jwtSettings.Audience, // Audiencia válida

            ValidateLifetime = true, // Valida la vigencia del token
            IssuerSigningKey = new SymmetricSecurityKey(key), // Clave secreta
            RequireExpirationTime = true, // No requiere tiempo de expiración
            ClockSkew = TimeSpan.Zero // No permite desfase de tiempo
        };
        
        // Registra los parámetros de validación como singleton
        services.AddSingleton(tokenValidationParameters);

        // Configura el esquema de autenticación JWT como predeterminado
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true; // Guarda el token en las propiedades de autenticación
            options.TokenValidationParameters = tokenValidationParameters; // Usa los parámetros configurados

             // Configuración adicional para debugging
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("JWT Token validated successfully");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine($"JWT Challenge: {context.ErrorDescription}");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}


