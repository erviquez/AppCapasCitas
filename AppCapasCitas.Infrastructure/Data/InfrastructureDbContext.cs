
using Microsoft.EntityFrameworkCore;
using AppCapasCitas.Domain.Models;
using System.Reflection;
namespace AppCapasCitas.Infrastructure.Data;

public class InfrastructureDbContext:DbContext
{

    public InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options) : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
    {
        property.SetColumnType("decimal(18,2)");
    }
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        

              
    }


}

