using System;
using System.ComponentModel.DataAnnotations;

namespace AppCapasCitas.Domain.Models.Common;

public abstract class EntidadBase
{
    [Key]
    public Guid Id { get; set; } 
}
    