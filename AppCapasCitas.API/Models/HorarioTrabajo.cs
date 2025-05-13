using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.API.Models.Common;

namespace AppCapasCitas.API.Models;

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
        public int MedicoId { get; set; }
        public virtual Medico? Medico { get; set; }
    }