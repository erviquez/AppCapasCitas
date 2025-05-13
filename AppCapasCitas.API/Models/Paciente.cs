using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.API.Models.Common;

namespace AppCapasCitas.API.Models;

public partial class Paciente: EntidadBaseAuditoria
    {

        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        [StringLength(20)]
        public string? Genero { get; set; }
        public string? Alergias { get; set; }
        public string? EnfermedadesCronicas { get; set; }
        public string? MedicamentosActuales { get; set; }

        // Relaciones
        public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
        public virtual ICollection<HistorialMedico> HistorialMedico { get; set; } = new HashSet<HistorialMedico>();
        public virtual ICollection<Pago> Pagos { get; set; } = new HashSet<Pago>();
        public virtual ICollection<RecetaMedica> RecetasMedicas { get; set; } = new HashSet<RecetaMedica>();
        public virtual Usuario? Usuario { get; set; }
    }