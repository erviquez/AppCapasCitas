using System.Net.Http.Json;
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
            var result = await _httpClient.GetFromJsonAsync<Response<List<MedicoResponse>>>("api/v1/Medico");
            
            if (result != null)
            {
                return result;
            }
            else
            {

                response.IsSuccess = false;
                response.Message = "No se encontraron medicos";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Exception", ex.Message) };
        }
        return response;
    }
}
