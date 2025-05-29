using System.Net.Http.Json;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;

namespace AppCapasCitas.FrontEnd.Proxy.Implementaciones;

public class UsuarioProxy : IUsuarioProxy
{
    private readonly HttpClient _httpClient;
    public UsuarioProxy(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Response<RegistrationResponse>> RegistrarUsuarioAsync(RegistrationRequest request)
    {
        var response = new Response<RegistrationResponse>();
        if (request == null)
        {
            response.IsSuccess = false;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "Error en el request recibido") };
        }

        var result = await _httpClient.PostAsJsonAsync<RegistrationRequest>("api/v1/Account/Register", request!);

        if (result.IsSuccessStatusCode)
        {

            response = await result.Content.ReadFromJsonAsync<Response<RegistrationResponse>>()
                   ?? new Response<RegistrationResponse>();
            return response;
        }

        response.IsSuccess = false;
        response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "Failed to register user") };
        return response;
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

        if (result.IsSuccessStatusCode)
        {
            if (!response.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = response.Message;
            }
            else
            {
                response = await result.Content.ReadFromJsonAsync<Response<AuthResponse>>()
                   ?? new Response<AuthResponse>();
            }            
            return response;
        }

        response.IsSuccess = false;
        response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "Failed to login user") };
        return response;
    }

}
