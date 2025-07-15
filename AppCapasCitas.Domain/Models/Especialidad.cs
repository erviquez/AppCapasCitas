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
        // Costo base por especialidad
        [Range(0.01, 9999.99)]          
        public decimal? CostoConsultaBase { get; set; }

        // Propiedades de navegación
        public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();
}
