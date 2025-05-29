using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface IUsuarioProxy
{
    Task<Response<RegistrationResponse>> RegistrarUsuarioAsync(RegistrationRequest request);
    Task<Response<AuthResponse>> LoginAsync(AuthRequest request);
}
