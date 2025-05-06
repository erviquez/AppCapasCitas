using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Hospital
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? TelefonoPrincipal { get; set; }

    public string? EmailContacto { get; set; }

    public string? SitioWeb { get; set; }

    public string Direccion { get; set; } = null!;

    public string? CodigoPostal { get; set; }

    public string? Municipio { get; set; }

    public string? Pais { get; set; }

    public string Url { get; set; } = null!;

    public string? HorarioAtencion { get; set; }

    public string? ServiciosEspeciales { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Consultorio> Consultorios { get; set; } = new List<Consultorio>();

    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();

    // public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
}
