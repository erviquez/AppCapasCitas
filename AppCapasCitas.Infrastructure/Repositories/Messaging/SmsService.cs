
using System.Text;
using System.Text.Json;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Transversal.Common;
using Microsoft.Extensions.Options;

namespace AppCapasCitas.Infrastructure.Repositories.Messaging;


public class SmsService : ISmsService
{
    private readonly HttpClient _httpClient;
    private readonly IAppLogger<SmsService> _appLogger;
    private readonly SmsSettings _smsSettings;
    // private const string AuthHeaderValue = "DataVP:d4T$v01C3.m4s1v05";
    // private const string ApiUrl = "https://appt.datavoice.com.mx/ContactBridgeAPI/api/SMS/SendSMSByIntegration";

    public SmsService(HttpClient httpClient, IAppLogger<SmsService> appLogger, IOptions<SmsSettings> smsSettings)
    {
        _httpClient = httpClient;
        _appLogger = appLogger;
        _smsSettings = smsSettings.Value;
    }

    public async Task<Response<bool>> SendSms(Sms sms)
    {
        var payload = new
        {
            idApiIntegration = 10,
            contact = sms.Contact,
            date = sms.Date.ToString("o"), // ISO 8601
            message = sms.Message
        };
        if(!_smsSettings.Active)
        {
            _appLogger.LogInformation("SMS service is inactive, skipping SMS sending.");
            return new Response<bool> { IsSuccess = false, Data = false, Message = "SMS service is inactive" };
        }
        
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, _smsSettings.ApiUrl);
        request.Headers.Add("Authentication-Header", _smsSettings.AuthHeaderValue);
        request.Content = content;

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return new Response<bool> { IsSuccess = true, Data = true, Message = "SMS enviado correctamente" };
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new Response<bool> { IsSuccess = false, Data = false, Message = $"Error enviando SMS: {error}" };
        }
    }
}