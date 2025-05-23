using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Infrastructure.Data;
using AppCapasCitas.Infrastructure.Persistence;
using AppCapasCitas.Infrastructure.Repositories;
using AppCapasCitas.Infrastructure.Repositories.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppCapasCitas.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InfrastructureDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"))
                .UseSnakeCaseNamingConvention()
            );
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        //identity
        //services.AddScoped<ICurrentUserService, CurrentUserService>();

        

        // var emailSettings = configuration.GetSection("EmailSettings");
        // Console.WriteLine(emailSettings?.Value); // Debería mostrar "edelaov@gmail.com"

        // // services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));      
        // services.Configure<EmailSettings>( c => configuration.GetSection("EmailSettings"));
        // // services.Configure<EmailSettings>(options =>
        // // {
        // //     options.Email = configuration["EmailSettings:Email"];
        // //     options.Secret = configuration["EmailSettings:Secret"];
        // //     options.Port = configuration["EmailSettings:Port"];
        // //     options.Server = configuration["EmailSettings:Server"];
        // // });

        services.AddTransient<IEmailService, EmailService>();
        return services;
    }

}
