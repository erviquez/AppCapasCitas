using System;

namespace AppCapasCitas.Application.Models.Identity;

public class SmsSettings
{
    public string? AuthHeaderValue { get; set; }
    public string? ApiUrl { get; set; }
    public bool Active { get; set; }
}
