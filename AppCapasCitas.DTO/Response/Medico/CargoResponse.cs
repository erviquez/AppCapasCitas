using System;
using AppCapasCitas.DTO.Response.Especialidad;

namespace AppCapasCitas.DTO.Response.Medico;

public class CargoResponse
{
    public Guid CargoId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public int NivelJerarquico { get; set; }
    public bool EsJefatura { get; set; }
    public EspecialidadResponse? EspecialidadResponse { get; set; }
    public List<MedicoEspecialidadHospitalResponse>? MedicoEspecialidadHospitalResponse { get; set; }
}
