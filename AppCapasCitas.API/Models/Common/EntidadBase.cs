using System;
using System.ComponentModel.DataAnnotations;

namespace AppCapasCitas.API.Models.Common;

public abstract class EntidadBase
{
    [Key]
    public int Id { get; set; }
}
