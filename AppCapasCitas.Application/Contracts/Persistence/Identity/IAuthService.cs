
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;


namespace AppCapasCitas.Application.Contracts.Persistence.Identity;

public interface IAuthService
{
    Task<Response<AuthResponse>> Login(AuthRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
    Task<Response<AuthResponse>> RefreshToken(TokenRequest tokenRequest);
    Task<bool> DeleteUser(string userId);
    Task<Response<AuthResponse>> Logout(LogoutRequest request);
    Task<int> CommitAsync();
    //
    Task<Guid> GetRoleIdByName(string roleName);
    Task<bool> AssignRoleToUser(string userId, string roleId); // Para compensación     
    Task<bool> RemoveRoleFromUser(string userId, string roleId); // Para compensación

}
