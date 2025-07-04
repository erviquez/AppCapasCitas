using System.Net.Http.Json;
using AppCapasCitas.DTO.Request.Medico;
using AppCapasCitas.DTO.Request.Usuario;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;

namespace AppCapasCitas.FrontEnd.Proxy.Implementaciones;

public class MedicoProxy : IMedicoProxy
{
    private readonly HttpClient _httpClient;
    public MedicoProxy(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    public async Task<Response<List<MedicoResponse>>> ObtenerMedicosAsync()
    {
        var response = new Response<List<MedicoResponse>>();
        try
        {
            var result = await _httpClient.GetAsync("api/v1/Medico/");
            if (result.IsSuccessStatusCode)
            {
                response = await result.Content.ReadFromJsonAsync<Response<List<MedicoResponse>>>();
                return response!;
            }
            else
            {

                response.IsSuccess = false;
                response.Message = "No se encontraron medicos";
            }
        }
        catch (Exception ex)
        {
            response!.IsSuccess = false;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Exception", ex.Message) };
        }
        return response;
    }


    public async Task<ResponsePagination<List<MedicoResponse>>> ObtenerPaginationMedicosAsync(
        string sort,
        int pageNumber,
        int pageSize,
        string searchText,
        string? isActive = null)
    {
        var response = new ResponsePagination<List<MedicoResponse>>();
        try
        {
            var url = $"api/v1/Medico/pagination{BuildQueryString(sort, pageNumber, pageSize, searchText, isActive)}";
            var result = await _httpClient.GetAsync(url);
            response = await result.Content.ReadFromJsonAsync<ResponsePagination<List<MedicoResponse>>>();
        }
        catch (Exception ex)
        {
            response!.IsSuccess = false;
            response.Message = "Error al obtener la paginación de usuarios" + ex.Message;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", ex.Message) };
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
        var result = await _httpClient.PutAsJsonAsync(url, usuarioRequest);
        response = await result.Content.ReadFromJsonAsync<Response<bool>>();
        return response!;
    }

    public async Task<Response<bool>> ActualizarMedicoAsync(MedicoRequest medicoRequest)
    {
        var response = new Response<bool>();
        if (medicoRequest == null)
        {
            response.IsSuccess = false;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", "Error en el request recibido") };
            return response;
        }
        var result = await _httpClient.PutAsJsonAsync("api/v1/Medico/", medicoRequest!);
        if (result.IsSuccessStatusCode)
        {
            response = await result.Content.ReadFromJsonAsync<Response<bool>>();
            return response!;
        }
        response.IsSuccess = false;
        response.Message = "No se pudo actualizar el médico";
        return response;
    }     
    
        public async Task<Response<MedicoResponse>> ObtenerMedicoPorIdAsync(Guid medicoId)
        {
            var response = new Response<MedicoResponse>();
            try
            {
                var result = await _httpClient.GetAsync($"api/v1/Medico/GetById/{medicoId}");
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadFromJsonAsync<Response<MedicoResponse>>();
                    return response!;
                }
                response.IsSuccess = false;
                response.Message = "No se pudo recuperar el usuario por ID";
                return response;    
            }
            catch (Exception ex)
            {
                response!.IsSuccess = false;
                response.Message = "Error al obtener el medico por ID: " + ex.Message;
                response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", ex.Message) };
            }
            return response;
        }
}
