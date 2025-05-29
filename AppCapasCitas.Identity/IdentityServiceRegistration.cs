using System.Text;
using AppCapasCitas.Application.Contracts.Identity;
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
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var sectionJwtSettings = configuration.GetSection("JwtSettings");
        // services.Configure<EmailSettings>( c => configuration.GetSection("EmailSettings"));

        // Configura el DbContext de Entity Framework para Identity con SQL Server
        // Especifica la cadena de conexión y el ensamblado donde están las migraciones
        services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
            b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)));

        // Configura ASP.NET Core Identity con ApplicationUser personalizado y IdentityRole
        // Esto establece la gestión de usuarios, roles y proveedores de tokens por defecto
        services.AddIdentity<ApplicationUser, IdentityRole>()
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
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Valida el emisor (issuer)
            ValidIssuer =  sectionJwtSettings["Issuer"], 
            ValidateIssuerSigningKey = true, // Valida la firma del emisor
            ValidateAudience = true, // Valida la audiencia (audience)
            ValidAudience = sectionJwtSettings["Audience"], // Audiencia válida
            ValidateLifetime = true, // Valida la vigencia del token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sectionJwtSettings["Key"]!)), // Clave secreta
            RequireExpirationTime = false, // No requiere tiempo de expiración
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
        });

        return services;
    }
}












// using System.Text;
// using AppCapasCitas.Application.Contracts.Identity;
// using AppCapasCitas.Application.Models.Identity;
// using AppCapasCitas.Identity.Data;
// using AppCapasCitas.Identity.Models;
// using AppCapasCitas.Identity.Policies;
// using AppCapasCitas.Identity.Services;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;

// namespace AppCapasCitas.Identity;

// public static class IdentityServiceRegistration
// {
//     public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
//     {
//         services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

//         // Configure DbContext with your custom ApplicationUser
//         services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
//             options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
//             b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)));

//         // Use ApplicationUser instead of IdentityUser
//         services.AddIdentity<ApplicationUser, IdentityRole>()
//                 .AddEntityFrameworkStores<CleanArchitectureIdentityDbContext>()
//                 .AddDefaultTokenProviders();

//         // Authorization policies
//         services.AddAuthorization(options =>
//         {
//             options.AddPolicy("RequireAdminRole", policy => 
//                 policy.RequireRole("Administrador"));
//             options.AddPolicy("RequireMedicoRole", policy => 
//                 policy.RequireRole("Medico"));
//             options.AddPolicy("RequireOperadorRole", policy => 
//                 policy.RequireRole("Operador"));
//             options.AddPolicy("RequirePacienteRole", policy => 
//                 policy.RequireRole("Paciente"));
                
//             options.AddPolicy("RequireEmailVerified", policy => 
//                 policy.RequireClaim("EmailVerified", "true"));
                
//             options.AddPolicy("Over18YearsOld", policy =>
//                 policy.Requirements.Add(new MinimumAgeRequirement(18)));
//         });

//         services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
//         services.AddTransient<IAuthService, AuthService>();
        
//         // JWT Configuration
//         var tokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             RequireExpirationTime = false,
//             ClockSkew = TimeSpan.Zero
//         };
        
//         services.AddSingleton(tokenValidationParameters);

//         services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         }).AddJwtBearer(options =>
//         {
//             options.SaveToken = true;
//             options.TokenValidationParameters = tokenValidationParameters;
//         });

//         return services;
//     }
// }










// using System.Text;
// using AppCapasCitas.Application.Contracts.Identity;
// using AppCapasCitas.Application.Models.Identity;
// using AppCapasCitas.Identity.Data;
// using AppCapasCitas.Identity.Models;
// using AppCapasCitas.Identity.Policies;
// using AppCapasCitas.Identity.Services;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;

// namespace AppCapasCitas.Identity;

// public static class IdentityServiceRegistration
// {
//     public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
//     {
//         services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

//         services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
//             options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
//             b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)));

//         services.AddIdentity<ApplicationUser, IdentityRole>()
//                 .AddEntityFrameworkStores<CleanArchitectureIdentityDbContext>()
//                 .AddDefaultTokenProviders();

//         // Configuración de políticas de autorización
//         services.AddAuthorization(options =>
//         {
//             // Política que requiere un rol específico
//             options.AddPolicy("RequireAdminRole", policy => 
//                 policy.RequireRole("Administrador"));
//             options.AddPolicy("RequireMedicoRole", policy => 
//                 policy.RequireRole("Medico"));
//             options.AddPolicy("RequireOperadorRole", policy => 
//                 policy.RequireRole("Operador"));
//             options.AddPolicy("RequirePacienteRole", policy => 
//                 policy.RequireRole("Paciente"));
                
//             // Política que requiere un claim específico
//             options.AddPolicy("RequireEmailVerified", policy => 
//                 policy.RequireClaim("EmailVerified", "true"));
                
//             //Política personalizada más compleja
//             options.AddPolicy("Over18YearsOld", policy =>
//                 policy.Requirements.Add(new MinimumAgeRequirement(18)));
                

//             // options.AddPolicy("OtraPolitica", ...);
//         });

//         services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
//         services.AddTransient<IAuthService, AuthService>();
        
//         var tokenValidationParamaters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             RequireExpirationTime = false,
//             ClockSkew = TimeSpan.Zero
//         };
        
//         services.AddSingleton(tokenValidationParamaters);

//         services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         }).AddJwtBearer(options =>
//         {
//             options.SaveToken = true;
//             options.TokenValidationParameters = tokenValidationParamaters;
//         });

//         return services;
//     }
// }


// using System.Text;
// using AppCapasCitas.Application.Contracts.Identity;
// using AppCapasCitas.Application.Models.Identity;
// using AppCapasCitas.Identity.Data;
// using AppCapasCitas.Identity.Services;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;

// namespace AppCapasCitas.Identity;

// public static class IdentityServiceRegistration
// {
//     public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
//     {
//         services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

//         services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
//             options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
//             b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)));

//         //
//         services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CleanArchitectureIdentityDbContext>();


//         services.AddTransient<IAuthService, AuthService>();
//         var tokenValidationParamaters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             RequireExpirationTime = false,
//             ClockSkew = TimeSpan.Zero
//         };
//         services.AddSingleton(tokenValidationParamaters);

//         services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         }).AddJwtBearer(options =>
//         {
//             options.SaveToken = true;
//             options.TokenValidationParameters = tokenValidationParamaters;

//         });
//         return services;

//     }
// }

