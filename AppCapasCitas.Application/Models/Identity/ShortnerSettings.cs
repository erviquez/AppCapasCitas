using System;

namespace AppCapasCitas.Application.Models.Identity;

public class ShortnerSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string? Host { get; set; } 
    public string ApiKey { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int ExpirationDays { get; set; } = 30;
    public bool IsEnabled { get; set; } = true;

}
