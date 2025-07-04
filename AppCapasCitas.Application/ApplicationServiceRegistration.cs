
using System.Reflection;
using AppCapasCitas.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AppCapasCitas.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(currentAssembly);
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            //Identity
           //services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
            return services;
    }

}
