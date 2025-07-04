
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppCapasCitas.FrontEnd.Security;

// 
public class AuthenticationService : AuthenticationStateProvider
{
    private readonly ISessionStorageService _session;
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _principal = new ClaimsPrincipal(new ClaimsIdentity());

    public AuthenticationService(ISessionStorageService session, HttpClient httpClient)
    {
        _session = session;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sesionUsuario = await _session.GetItemAsync<AuthResponse>("session");
        if (sesionUsuario == null)
        {
            return new AuthenticationState(_principal);
        }
        else
        {
            // Solo asigna el token si es diferente
            if (_httpClient.DefaultRequestHeaders.Authorization == null ||
                _httpClient.DefaultRequestHeaders.Authorization.Parameter != sesionUsuario.Token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sesionUsuario.Token);
            }
            var jwt = LeerToken(sesionUsuario);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, authenticationType: "JWT"));
            return new AuthenticationState(claims);
        }
    }

    public async Task Autenticar(AuthResponse response)
    {
        ClaimsPrincipal principal;
        if (response != null && response.Token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);
            await _session.SetItemAsync("session", response);
            // var jwt = LeerToken(response);
            // principal = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, authenticationType: "JWT"));
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        else
        {
            await _session.RemoveItemAsync("session");
            principal = _principal;
        }
    }

    private JwtSecurityToken LeerToken(AuthResponse response)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(response.Token);
        return jwtToken;
    }

    public async Task Logout()
    {
        var sesionUsuario = await _session.GetItemAsync<AuthResponse>("session");
        if (sesionUsuario != null)
        {
            var logoutRequest = new LogoutRequest
            {
                Token = sesionUsuario.RefreshToken,
                UserId = sesionUsuario.Id
            };
            var response = await _httpClient.PostAsJsonAsync("api/v1/Account/Logout", logoutRequest);
        }
        await _session.RemoveItemAsync("session");
        _httpClient.DefaultRequestHeaders.Authorization = null;
        _principal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    //GetUserActual
    public async Task<AuthResponse> GetUserActual()
    {
        var sesionUsuario = await _session.GetItemAsync<AuthResponse>("session");
        if (sesionUsuario != null)
        {
            return sesionUsuario;
        }
        return null!;
    }
}
