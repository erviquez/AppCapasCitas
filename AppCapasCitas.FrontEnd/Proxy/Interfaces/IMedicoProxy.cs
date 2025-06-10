using System;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface IMedicoProxy
{
    Task<Response<List<MedicoResponse>>> ObtenerMedicosAsync();
}
