
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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        

              
    }


}

