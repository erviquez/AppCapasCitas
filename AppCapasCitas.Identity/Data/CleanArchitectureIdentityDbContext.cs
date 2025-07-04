
using AppCapasCitas.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppCapasCitas.Identity.Data;
//public class CleanArchitectureIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>

public class CleanArchitectureIdentityDbContext : IdentityDbContext<ApplicationUser,ApplicationRole, string>
{
    public CleanArchitectureIdentityDbContext(DbContextOptions<CleanArchitectureIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //
        // Configure ApplicationUser properties
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.Active)
                .HasDefaultValue(true);
                
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETUTCDATE()");
                
            entity.Property(e => e.LastModifiedByDate)
                .HasDefaultValueSql("GETUTCDATE()");
                
            entity.Property(e => e.CreateBy)
                .HasMaxLength(100);
                
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100);
        });

    }

    public virtual DbSet<RefreshToken>? RefreshTokens{get;set;}

    //modelos render

    

}