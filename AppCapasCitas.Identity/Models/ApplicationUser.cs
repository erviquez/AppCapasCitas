
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppCapasCitas.Identity.Models;

public class ApplicationUser : IdentityUser
{

    public bool Active { get; set; } = true;

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public string? CreateBy { get; set; } = "system";

    public DateTime LastModifiedByDate { get; set; }


    public string? LastModifiedBy { get; set; }
}