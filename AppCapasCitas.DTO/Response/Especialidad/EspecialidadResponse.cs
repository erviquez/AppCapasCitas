using System;
using AppCapasCitas.DTO.Response.Hospital;
using AppCapasCitas.DTO.Response.Medico;

namespace AppCapasCitas.DTO.Response.Especialidad
;

public class EspecialidadResponse
{
        public Guid EspecialidadId { get; set; }
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }
        public decimal? CostoConsultaBase { get; set; }
        public bool Activo { get; set; }
        

        public HospitalResponse? HospitalResponse { get; set; } // Lista de hospitales asociados a la especialidad
}
