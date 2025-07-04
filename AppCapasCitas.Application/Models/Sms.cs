using System;

namespace AppCapasCitas.Application.Models;

public class Sms
{
    public string Password { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public int IdApiIntegration { get; set; }
    public string? Contact { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; } = string.Empty;

}