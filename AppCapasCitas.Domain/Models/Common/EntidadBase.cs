using System;
using System.ComponentModel.DataAnnotations;

namespace AppCapasCitas.Domain.Models.Common;

public abstract class EntidadBase
{
    [Key]
    public int Id { get; set; }
}
