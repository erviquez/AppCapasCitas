using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Especialidad : EntidadBaseAuditoria
{
        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }
 

        // Propiedades de navegación
        public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();
}
