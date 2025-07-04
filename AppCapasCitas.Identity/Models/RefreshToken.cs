using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppCapasCitas.Identity.Models;

public class RefreshToken:BaseDomainModel
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
    public string? JwtId { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime  ExpireDate { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }
}