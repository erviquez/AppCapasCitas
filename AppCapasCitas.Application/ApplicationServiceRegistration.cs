
using System.Reflection;
using AppCapasCitas.Application.Behaviours;
using AppCapasCitas.Application.Models.Identity;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace AppCapasCitas.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(currentAssembly);
            });
            //Deendency Injection
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            //Configuration
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));
            services.Configure<ShortnerSettings>(configuration.GetSection("ShortnerSettings"));
            services.Configure<UrlsConfirmationSettings>(configuration.GetSection("UrlsConfirmationSettings"));
            return services;
    }

}
