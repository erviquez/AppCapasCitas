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
    public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Configure DbContext with your custom ApplicationUser
        services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"),
            b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)));

        // Use ApplicationUser instead of IdentityUser
        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CleanArchitectureIdentityDbContext>()
                .AddDefaultTokenProviders();

        // Authorization policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => 
                policy.RequireRole("Administrador"));
            options.AddPolicy("RequireMedicoRole", policy => 
                policy.RequireRole("Medico"));
            options.AddPolicy("RequireOperadorRole", policy => 
                policy.RequireRole("Operador"));
            options.AddPolicy("RequirePacienteRole", policy => 
                policy.RequireRole("Paciente"));
                
            options.AddPolicy("RequireEmailVerified", policy => 
                policy.RequireClaim("EmailVerified", "true"));
                
            options.AddPolicy("Over18YearsOld", policy =>
                policy.Requirements.Add(new MinimumAgeRequirement(18)));
        });

        services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        services.AddTransient<IAuthService, AuthService>();
        
        // JWT Configuration
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero
        };
        
        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
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

