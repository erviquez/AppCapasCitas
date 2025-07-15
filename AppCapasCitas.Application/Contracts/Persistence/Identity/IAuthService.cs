
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Identity;


namespace AppCapasCitas.Application.Contracts.Persistence.Identity;

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
    Task<Response<string>> GetRoleIdByRoleId(string roleId);
    Task<bool> RemoveAllRolesFromUser(string userId);
    Task<Response<List<Role>>> GetRoles();
    Task<Response<List<string>>> GetRolesByUserId(string userId);

    Task<bool> UserExists(string userId);
    Task<Response<AuthResponse>> GetApplicationUser(string userId);
    Task<Response<IReadOnlyList<AuthResponse>>> GetAllApplicationUser();
    Task<Response<IReadOnlyList<AuthResponse>>> GetAllApplicationUserActive(bool active = true);
    Task<Response<bool>> UpdateApplicationUser(AuthRequest authRequest);
    Task<Response<bool>> AssignRoleToUser(string userId, string roleId); // Para compensación     
    Task<bool> RemoveRoleFromUser(string userId, string roleId); // Para compensación

    Task<Response<Guid>> ConfirmEmail(string userId, string token);
    Task<Response<Guid>> ConfirmPhoneNumber(string userId, string token);

    // Task<Response<AuthResponse>> Login(AuthRequest request);
    // Task<RegistrationResponse> Register(RegistrationRequest request);
    // Task<Response<AuthResponse>> RefreshToken(TokenRequest tokenRequest);
    // Task<bool> DeleteUser(string userId);
    // Task<Response<AuthResponse>> Logout(LogoutRequest request);
    // Task<int> CommitAsync();
    // //
    // Task<Guid> GetRoleIdByName(string roleName);
    // Task<bool> AssignRoleToUser(string userId, string roleId); // Para compensación     
    // Task<bool> RemoveRoleFromUser(string userId, string roleId); // Para compensación

}
