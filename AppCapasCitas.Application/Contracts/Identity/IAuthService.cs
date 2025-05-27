using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;
namespace AppCapasCitas.Application.Contracts.Identity;

public interface IAuthService
{
    Task<Response<AuthResponse>> Login(AuthRequest request);
    Task<Response<bool>> Logout(LogoutRequest request);    
    Task<Response<RegistrationResponse>> Register(RegistrationRequest request);
    Task<Response<AuthResponse>> RefreshToken(TokenRequest tokenRequest);
    Task<Response<bool>> DeleteUser(string userId);
    Task<int> CommitAsync();
    //
    Task<Guid> GetRoleIdByName(string roleName);
    Task<string> GetRoleIdByRoleId(string roleId);

    Task<bool> UserExists(string userId);
    Task<Response<AuthResponse>> GetApplicationUser(string userId);
    Task<Response<IReadOnlyList<AuthResponse>>> GetAllApplicationUser();
    Task<Response<bool>> UpdateApplicationUser(AuthRequest authRequest);
    Task<Response<bool>> AssignRoleToUser(string userId, string roleId); // Para compensación     
    Task<bool> RemoveRoleFromUser(string userId, string roleId); // Para compensación

}
