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

    //registrar ultimo login
    // private async Task<Response<bool>> RegistrarUltimoLoginAsync(string userId)
    // {
    //     var response = new Response<bool>();
    //     if (string.IsNullOrEmpty(userId))
    //     {
    //         response.IsSuccess = false;
    //         response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "El userId no puede ser nulo o vacío") };
    //         return response;
    //     }

    //     var result = await _httpClient.PostAsJsonAsync("api/v1/Account/RegistrarUltimoLogin", userId);
    //     response = await result.Content.ReadFromJsonAsync<Response<bool>>()
    //             ?? new Response<bool>();

    //     return response;
    // }

    public Task<Response<RegistrationResponse>> RegistrarUsuarioAsync(RegistrationRequest request)
    {
        throw new NotImplementedException();
    }
}
