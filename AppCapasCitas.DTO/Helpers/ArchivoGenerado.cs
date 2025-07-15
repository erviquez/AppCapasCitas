using System;

namespace AppCapasCitas.DTO.Helpers;

public class ArchivoGenerado
{
    public string NombreArchivo { get; set; } = string.Empty;
    public byte[] Contenido { get; set; } = Array.Empty<byte>();
    public string TipoContenido { get; set; } = string.Empty;
    public long Tamaño => Contenido?.Length ?? 0;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

}
