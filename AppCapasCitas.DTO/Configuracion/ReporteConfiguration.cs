namespace AppCapasCitas.DTO.Configuration;

public class ReporteConfiguration
{
    public string? Empresa { get; set; }
    public string? LogoPath { get; set; }
    public string? LogoBase64 { get; set; }
    public string? ImagenFondoPath { get; set; }
    public string? ImagenFondoBase64 { get; set; }
    public int TamanoPagina { get; set; } = 1; // A4
    public int Orientacion { get; set; } = 0; // Vertical
    public string TipoFuente { get; set; } = "Arial";
    public float TamanoFuenteTitulo { get; set; } = 18f;
    public float TamanoFuenteSubtitulo { get; set; } = 14f;
    public float TamanoFuenteTexto { get; set; } = 10f;
    public float TamanoFuenteTabla { get; set; } = 9f;
    public string ColorTitulo { get; set; } = "#2E86C1";
    public string ColorTexto { get; set; } = "#333333";
    public string ColorEncabezadoTabla { get; set; } = "#E8F4FD";
    public float OpacidadFondo { get; set; } = 1.0f;
    public string? TextoMarcaAgua { get; set; } = "Reporte configuracion";
    public float MargenSuperior { get; set; } = 50f;
    public float MargenInferior { get; set; } = 50f;
    public float MargenIzquierdo { get; set; } = 50f;
    public float MargenDerecho { get; set; } = 50f;
    public bool MostrarNumeroPagina { get; set; } = true;
    public bool MostrarFechaGeneracion { get; set; } = true;
    public string? PiePagina { get; set; } = "Sistema de Gestión de Citas Médicas";

    
}

public enum TamanoPagina
{
    A4,
    A3,
    A5,
    Letter,
    Legal,
    Tabloid
}

public enum OrientacionPagina
{
    Vertical,
    Horizontal
}