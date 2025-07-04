using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class HorarioTrabajo : EntidadBaseAuditoria
    {
        [Required]
        public DayOfWeek DiaSemana { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFin { get; set; }

    // Relaciones
        [Required]
        public Guid MedicoId { get; set; }
        public virtual Medico? MedicoNavigation { get; set; }
    }