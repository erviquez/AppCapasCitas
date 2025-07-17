using System.Net.Http.Json;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;

namespace AppCapasCitas.FrontEnd.Proxy.Implementaciones;

public class LoginProxy:ILoginProxy
{
    private readonly HttpClient _httpClient;

    public LoginProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    //login
    public async Task<Response<AuthResponse>> LoginAsync(AuthRequest request)
    {
        var response = new Response<AuthResponse>();
        if (request == null)
        {
            response.IsSuccess = false;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "Error en el request recibido") };
            return response;
        }

        var result = await _httpClient.PostAsJsonAsync<AuthRequest>("api/v1/Account/Login", request!);
        response = await result.Content.ReadFromJsonAsync<Response<AuthResponse>>()
                ?? new Response<AuthResponse>();


            return response;


    }
    /// <summary>
    /// /Reset password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<bool>> ResetPasswordAsync(AuthResetPasswordRequest request)
    {
        var response = new Response<bool>();
        if (request == null)
        {
            response.IsSuccess = false;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "Error en el request recibido") };
            return response;
        }

        var result = await _httpClient.PostAsJsonAsync<AuthResetPasswordRequest>("api/v1/Account/ResetPassword", request!);
        response = await result.Content.ReadFromJsonAsync<Response<bool>>()
                ?? new Response<bool>();

        return response;
    }


    public Task<Response<RegistrationResponse>> RegistrarUsuarioAsync(RegistrationRequest request)
    {
        throw new NotImplementedException();
    }
}
