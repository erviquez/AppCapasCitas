using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Request.Usuario;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Identity;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface IUsuarioProxy
{
    Task<Response<RegistrationResponse>> RegistrarUsuarioAsync(RegistrationRequest request);

    //Lista de usuarios
    Task<Response<List<UsuarioResponse>>> ObtenerUsuariosAsync();
    Task<Response<UsuarioResponse>> ObtenerUsuarioAsync(Guid usuarioId);
    Task<ResponsePagination<List<UsuarioResponse>>> ObtenerPaginationUsuariosAsync(
                                                                        string sort,
                                                                        int pageNumber,
                                                                        int pageSize,
                                                                        string searchText,
                                                                        string? isActive = null);

    Task<bool> LogoutAsync(LogoutRequest request);
    Task<Response<bool>> DisableUsuarioByIdAsync(UsuarioRequest usuarioRequest);
    Task<Response<bool>> ConvertUsuarioAsync(UsuarioConvertRequest usuarioConvertRequest);
    Task<Response<List<Role>>> ObtenerRolesAsync();
    Task<Response<bool>> ActualizarUsuarioAsync(UsuarioRequest usuarioRequest);
    Task<Response<UsuarioResponse>> ObtenerUsuarioPorIdAsync(Guid usuarioId);
   
}
