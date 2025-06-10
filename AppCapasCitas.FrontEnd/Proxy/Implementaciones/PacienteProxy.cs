using System;
using AppCapasCitas.DTO.Request.Paciente;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Implementaciones;

public class PacienteProxy : IPacienteProxy
{
    public Task<Response<PacienteResponse>> ObtenerPacientesAsync()
    {
        throw new NotImplementedException();
    }
}
