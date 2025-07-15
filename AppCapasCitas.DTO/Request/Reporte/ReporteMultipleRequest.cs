using System;
using AppCapasCitas.DTO.Configuration;

namespace AppCapasCitas.DTO.Request.Reporte;

public class ReporteMultipleRequest
{
    public List<string> Ids { get; set; } = new();
    public bool GenerarZip { get; set; } = true;
    public ReporteConfiguration? ConfiguracionImpresion { get; set; }
    public string? NombreArchivo { get; set; }
}
