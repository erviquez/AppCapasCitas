using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.DTO.Configuration;
using AppCapasCitas.Reporting.Helpers;
using AppCapasCitas.Reporting.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppCapasCitas.Reporting;

public static class ReportingServiceRegistration
{
    public static IServiceCollection AddReportingServices(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddTransient<IReporteService, ReporteService>();
        var reporteConfig = configuration.GetSection("ReporteSettings");
        services.Configure<ReporteConfiguration>(reporteConfig);
        services.AddTransient<PdfAddDefaultSettings>();
        return services;
    }
}