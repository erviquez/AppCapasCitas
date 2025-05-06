using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public Guid IdentityId { get; set; }

    public Guid? RolId { get; set; }

    public string RolName { get; set; } = null!;

    public bool Activo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public int CodigoPais { get; set; }

    public string Pais { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime? UltimoLogin { get; set; }

    public int? PacienteId { get; set; }

    public int? MedicoId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public string Email { get; set; } = null!;

    public virtual Medico? Medico { get; set; }

    public virtual Paciente? Paciente { get; set; }
}
