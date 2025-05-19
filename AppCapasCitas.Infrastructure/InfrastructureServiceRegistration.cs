using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Infrastructure.Data;
using AppCapasCitas.Infrastructure.Persistence;
using AppCapasCitas.Infrastructure.Repositories;
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

        return services;


    }

}
