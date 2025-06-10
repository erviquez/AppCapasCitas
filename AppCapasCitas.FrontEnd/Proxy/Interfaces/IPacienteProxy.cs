using System;
using AppCapasCitas.DTO.Request.Paciente;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface IPacienteProxy
{
    Task<Response<PacienteResponse>> ObtenerPacientesAsync();
}
