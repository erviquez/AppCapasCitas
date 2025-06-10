using System;

namespace AppCapasCitas.Identity.Models;

public class BaseDomainModel
{
    public int Id {get;set;}
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; } = "system";
    public DateTime LastModifiedByDate { get; set; }
    public string? LastModifiedBy { get; set; }
}
