
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class HistorialMedico: EntidadBaseAuditoria
    {
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string? Diagnostico { get; set; }

        public string? Tratamiento { get; set; }
        public string? Notas { get; set; }
        public string? PresionArterial { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }

        // Relaciones
        public Guid PacienteId { get; set; }
        public virtual Paciente? PacienteNavigation { get; set; }



        public Guid MedicoId { get; set; }          
        public virtual Medico? MedicoNavigation { get; set; }

        public Guid CitaId { get; set; }
        public virtual Cita? CitaNavigation { get; set; }
    }