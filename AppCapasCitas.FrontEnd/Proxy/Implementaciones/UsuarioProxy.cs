using System.Net.Http.Json;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Request.Usuario;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.DTO.Response.Usuario;
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

    public async Task<Response<List<UsuarioResponse>>> ObtenerUsuariosAsync()
    {
        var response = new Response<List<UsuarioResponse>>();
        var result = await _httpClient.GetAsync("api/v1/Usuario/");
        if (result.IsSuccessStatusCode)
        {
            response = await result.Content.ReadFromJsonAsync<Response<List<UsuarioResponse>>>();
            return response!;
        }
        response.IsSuccess = false;
        response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "No se pudieron recuperar los usuarios") };
        return response;


    }

    public async Task<ResponsePagination<List<UsuarioResponse>>> ObtenerPaginationUsuariosAsync(
        string sort,
        int pageNumber,
        int pageSize,
        string searchText,
        string? isActive = null)
    {
        var response = new ResponsePagination<List<UsuarioResponse>>();
        try
        {
            var url = $"api/v1/Usuario/pagination{BuildQueryString(sort, pageNumber, pageSize, searchText, isActive)}";
            var result = await _httpClient.GetAsync(url);
            response = await result.Content.ReadFromJsonAsync<ResponsePagination<List<UsuarioResponse>>>();
        }
        catch (Exception ex)
        {
            response!.IsSuccess = false;
            response.Message = $"Error: An error occurred while fetching paginated users: {ex.Message}";
        }
        return response!;
    }

    // Método auxiliar para construir el query string 
    private string BuildQueryString(string sort, int pageNumber, int pageSize, string searchText, string? isActive)
    {
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrEmpty(sort))
            query["sort"] = sort;
        if (pageNumber > 0)
            query["pageIndex"] = pageNumber.ToString();
        if (pageSize > 0)
            query["pageSize"] = pageSize.ToString();
        if (!string.IsNullOrEmpty(searchText))
            query["search"] = searchText;
        if (!string.IsNullOrEmpty(isActive))
            query["isActive"] = isActive;
        var queryString = query.ToString();
        return string.IsNullOrEmpty(queryString) ? string.Empty : $"?{queryString}";
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
        response = await result.Content.ReadFromJsonAsync<Response<RegistrationResponse>>();
        return response!;
    }

    public async Task<bool> LogoutAsync(LogoutRequest request)
    {
        var response = new Response<bool>();
        if (request == null)
        {
            response.IsSuccess = false;
            response.Message = "Logout request cannot be null";
            return false;
        }

        var result = await _httpClient.PostAsJsonAsync("api/v1/Account/Logout", request);
        return result.IsSuccessStatusCode;
    }
    public async Task<Response<bool>> DisableUsuarioByIdAsync(UsuarioRequest usuarioRequest)
    {       
        var response = new Response<bool>();

        if (usuarioRequest == null)
        {
            response.IsSuccess = false;
            response.Message = "El request no puede ser nulo";
            return response;
        }

        var url = $"api/v1/Account/DisableUsuarioById";
        var result = await _httpClient.PutAsJsonAsync(url,usuarioRequest);
        response = await result.Content.ReadFromJsonAsync<Response<bool>>();
        return response!;
    }

}
