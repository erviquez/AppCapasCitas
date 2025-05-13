using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Consultorio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Ubicacion { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? NumeroConsultorio { get; set; }

    public string? Equipamiento { get; set; }


    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    //Relaciones
    public int HospitalId { get; set; }
    public virtual Hospital Hospital { get; set; } = null!;
    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
