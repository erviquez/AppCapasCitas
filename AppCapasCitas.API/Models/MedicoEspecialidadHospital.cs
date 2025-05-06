using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class MedicoEspecialidadHospital
{
    public int Id { get; set; }

        // Claves foráneas
    public int MedicoId { get; set; }
    public int EspecialidadId { get; set; }
    public int HospitalId { get; set; }
    

    public int CargoId { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool Activo { get; set; }

    public decimal? CostoConsultaEspecifico { get; set; }

    public string? Consultorio { get; set; }

    public string? HorarioAtencion { get; set; }

    public string? NumeroContrato { get; set; }

    public string? TipoContratacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public virtual Cargo Cargo { get; set; } = null!;

     // Propiedades de navegación
    public virtual Especialidad Especialidad { get; set; } = null!;

    public virtual Hospital Hospital { get; set; } = null!;

    public virtual Medico Medico { get; set; } = null!;
}
