using System;

namespace AppCapasCitas.DTO.Response.Reporte;

public class ReporteResponse
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/pdf";
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
    public string Base64Content { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
    public int TotalRecords { get; set; }
}
