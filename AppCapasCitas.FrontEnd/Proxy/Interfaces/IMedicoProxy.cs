using AppCapasCitas.DTO.Request.Medico;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface IMedicoProxy
{
    Task<Response<List<MedicoResponse>>> ObtenerMedicosAsync();
    Task<ResponsePagination<List<MedicoResponse>>> ObtenerPaginationMedicosAsync(
        string sort,
        int pageNumber,
        int pageSize,
        string searchText,
        string? isActive = null);
    Task<Response<bool>> ActualizarMedicoAsync(MedicoRequest medicoRequest);
    Task<Response<MedicoResponse>> ObtenerMedicoPorIdAsync(Guid medicoId);

    
}
