using AppCapasCitas.DTO.Configuration;
using Microsoft.Extensions.Options;

namespace AppCapasCitas.Reporting.Helpers;

public class PdfAddDefaultSettings
{
    private readonly ReporteConfiguration _impresionConfig;

    public PdfAddDefaultSettings(IOptions<ReporteConfiguration> impresionConfig)
    {
        _impresionConfig = impresionConfig.Value;
    }

    /// <summary>
    /// Crea una configuración de impresión con valores por defecto desde appsettings.json
    /// </summary>
    /// <returns>ReporteSettings con valores por defecto</returns>
    public ReporteConfiguration CrearConfiguracionDefecto()
    {
        var defaults = _impresionConfig;
        
        return new ReporteConfiguration
        {
            TamanoPagina = defaults.TamanoPagina,
            Orientacion = defaults.Orientacion,
            TipoFuente = defaults.TipoFuente,
            TamanoFuenteTitulo = defaults.TamanoFuenteTitulo,
            TamanoFuenteSubtitulo = defaults.TamanoFuenteSubtitulo,
            TamanoFuenteTexto = defaults.TamanoFuenteTexto,
            TamanoFuenteTabla = defaults.TamanoFuenteTabla,
            ColorTitulo = defaults.ColorTitulo,
            ColorTexto = defaults.ColorTexto,
            ColorEncabezadoTabla = defaults.ColorEncabezadoTabla,
            LogoBase64 = ObtenerLogoBase64(),
            ImagenFondoBase64 = ObtenerImagenFondoBase64(),
            LogoPath = defaults.LogoPath,
            ImagenFondoPath = defaults.ImagenFondoPath,
            OpacidadFondo = defaults.OpacidadFondo,
            MargenSuperior = defaults.MargenSuperior,
            MargenInferior = defaults.MargenInferior,
            MargenIzquierdo = defaults.MargenIzquierdo,
            MargenDerecho = defaults.MargenDerecho,
            MostrarNumeroPagina = defaults.MostrarNumeroPagina,
            MostrarFechaGeneracion = defaults.MostrarFechaGeneracion,
            PiePagina = defaults.PiePagina,
            TextoMarcaAgua = defaults.TextoMarcaAgua
        };
    }

    /// <summary>
    /// Aplica configuración por defecto a una configuración existente (solo valores nulos/vacíos)
    /// </summary>
    /// <param name="configuracion">Configuración existente</param>
    /// <returns>Configuración con valores por defecto aplicados</returns>
    public ReporteConfiguration AplicarConfiguracionDefault()
    {
        var configuracion  = CrearConfiguracionDefecto();

        var defaults = _impresionConfig;

        // Aplicar valores por defecto solo si están vacíos/nulos
        configuracion.TipoFuente = string.IsNullOrEmpty(configuracion.TipoFuente) 
            ? defaults.TipoFuente 
            : configuracion.TipoFuente;

        configuracion.TamanoFuenteTitulo = configuracion.TamanoFuenteTitulo <= 0 
            ? defaults.TamanoFuenteTitulo 
            : configuracion.TamanoFuenteTitulo;

        configuracion.TamanoFuenteSubtitulo = configuracion.TamanoFuenteSubtitulo <= 0 
            ? defaults.TamanoFuenteSubtitulo 
            : configuracion.TamanoFuenteSubtitulo;

        configuracion.TamanoFuenteTexto = configuracion.TamanoFuenteTexto <= 0 
            ? defaults.TamanoFuenteTexto 
            : configuracion.TamanoFuenteTexto;

        configuracion.TamanoFuenteTabla = configuracion.TamanoFuenteTabla <= 0 
            ? defaults.TamanoFuenteTabla 
            : configuracion.TamanoFuenteTabla;

        configuracion.ColorTitulo = string.IsNullOrEmpty(configuracion.ColorTitulo) 
            ? defaults.ColorTitulo 
            : configuracion.ColorTitulo;

        configuracion.ColorTexto = string.IsNullOrEmpty(configuracion.ColorTexto) 
            ? defaults.ColorTexto 
            : configuracion.ColorTexto;

        configuracion.ColorEncabezadoTabla = string.IsNullOrEmpty(configuracion.ColorEncabezadoTabla) 
            ? defaults.ColorEncabezadoTabla 
            : configuracion.ColorEncabezadoTabla;

        configuracion.LogoBase64 = string.IsNullOrEmpty(configuracion.LogoBase64) 
            ? ObtenerLogoBase64() 
            : configuracion.LogoBase64;

        configuracion.ImagenFondoBase64 = string.IsNullOrEmpty(configuracion.ImagenFondoBase64) 
            ? ObtenerImagenFondoBase64() 
            : configuracion.ImagenFondoBase64;

        configuracion.OpacidadFondo = configuracion.OpacidadFondo <= 0 
            ? defaults.OpacidadFondo 
            : configuracion.OpacidadFondo;

        configuracion.MargenSuperior = configuracion.MargenSuperior <= 0 
            ? defaults.MargenSuperior 
            : configuracion.MargenSuperior;

        configuracion.MargenInferior = configuracion.MargenInferior <= 0 
            ? defaults.MargenInferior 
            : configuracion.MargenInferior;

        configuracion.MargenIzquierdo = configuracion.MargenIzquierdo <= 0 
            ? defaults.MargenIzquierdo 
            : configuracion.MargenIzquierdo;

        configuracion.MargenDerecho = configuracion.MargenDerecho <= 0 
            ? defaults.MargenDerecho 
            : configuracion.MargenDerecho;
        configuracion.MostrarNumeroPagina = configuracion.MostrarNumeroPagina == false 
            ? defaults.MostrarNumeroPagina
            : configuracion.MostrarNumeroPagina;
        configuracion.TextoMarcaAgua = string.IsNullOrEmpty(configuracion.TextoMarcaAgua) 
            ? defaults.TextoMarcaAgua 
            : configuracion.TextoMarcaAgua;
        configuracion.Orientacion = configuracion.Orientacion == 0 
            ? defaults.Orientacion 
            : configuracion.Orientacion;


        return configuracion;
    }

    /// <summary>
    /// Crea configuración específica para diferentes tipos de reportes
    /// </summary>
    /// <param name="tipoReporte">Tipo de reporte (ejecutivo, operativo, técnico)</param>
    /// <returns>Configuración optimizada para el tipo de reporte</returns>
    public ReporteConfiguration CrearConfiguracionPorTipo(TipoReporte tipoReporte)
    {
        var configuracionBase = CrearConfiguracionDefecto();

        return tipoReporte switch
        {
            TipoReporte.Ejecutivo => ConfiguracionEjecutiva(configuracionBase),
            TipoReporte.Operativo => ConfiguracionOperativa(configuracionBase),
            TipoReporte.Tecnico => ConfiguracionTecnica(configuracionBase),
            TipoReporte.Compacto => ConfiguracionCompacta(configuracionBase),
            _ => configuracionBase
        };
    }

    /// <summary>
    /// Valida que la configuración tenga valores válidos
    /// </summary>
    /// <param name="configuracion">Configuración a validar</param>
    /// <returns>Lista de errores de validación</returns>
    public List<string> ValidarConfiguracion(ReporteConfiguration configuracion)
    {
        var errores = new List<string>();

        if (configuracion.TamanoFuenteTitulo < 8 || configuracion.TamanoFuenteTitulo > 48)
            errores.Add("El tamano de fuente del título debe estar entre 8 y 48 puntos");

        if (configuracion.TamanoFuenteTexto < 6 || configuracion.TamanoFuenteTexto > 24)
            errores.Add("El tamano de fuente del texto debe estar entre 6 y 24 puntos");

        if (configuracion.OpacidadFondo < 0 || configuracion.OpacidadFondo > 1)
            errores.Add("La opacidad del fondo debe estar entre 0 y 1");

        if (configuracion.MargenSuperior < 10 || configuracion.MargenSuperior > 200)
            errores.Add("El margen superior debe estar entre 10 y 200 puntos");

        if (!EsColorValido(configuracion.ColorTitulo))
            errores.Add("El color del título no es válido");

        if (!EsColorValido(configuracion.ColorTexto))
            errores.Add("El color del texto no es válido");

        return errores;
    }

    /// <summary>
    /// Obtiene la configuración actual del sistema
    /// </summary>
    /// <returns>Configuración del sistema desde appsettings</returns>
    public ReporteConfiguration ObtenerConfiguracionSistema()
    {
        return _impresionConfig;
    }

    // Métodos privados de utilidad

    private string? ObtenerLogoBase64()
    {
        if (!string.IsNullOrEmpty(_impresionConfig.LogoBase64))
        {
            return _impresionConfig.LogoBase64;
        }

        if (!string.IsNullOrEmpty(_impresionConfig.LogoPath) && File.Exists(_impresionConfig.LogoPath))
        {
            try
            {
                var logoBytes = File.ReadAllBytes(_impresionConfig.LogoPath);
                return Convert.ToBase64String(logoBytes);
            }
            catch
            {
                // Si hay error leyendo el archivo, retornar null
                return null;
            }
        }

        return null;
    }

    private string? ObtenerImagenFondoBase64()
    {
        if (!string.IsNullOrEmpty(_impresionConfig.ImagenFondoBase64))
        {
            return _impresionConfig.ImagenFondoBase64;
        }

        if (!string.IsNullOrEmpty(_impresionConfig.ImagenFondoPath) && File.Exists(_impresionConfig.ImagenFondoPath))
        {
            try
            {
                var imagenBytes = File.ReadAllBytes(_impresionConfig.ImagenFondoPath);
                return Convert.ToBase64String(imagenBytes);
            }
            catch
            {
                return null;
            }
        }

        return null;
    }

    private static ReporteConfiguration ConfiguracionEjecutiva(ReporteConfiguration config)
    {
        config.TamanoFuenteTitulo = 22f;
        config.TamanoFuenteSubtitulo = 16f;
        config.ColorTitulo = "#1B4F72";
        config.ColorEncabezadoTabla = "#D5DBDB";
        config.MargenSuperior = 70f;
        config.MargenInferior = 70f;
        return config;
    }

    private static ReporteConfiguration ConfiguracionOperativa(ReporteConfiguration config)
    {
        config.TamanoFuenteTitulo = 18f;
        config.TamanoFuenteTexto = 10f;
        config.ColorTitulo = "#2E86C1";
        config.ColorEncabezadoTabla = "#E8F4FD";
        return config;
    }

    private static ReporteConfiguration ConfiguracionTecnica(ReporteConfiguration config)
    {
        config.TipoFuente = "Courier";
        config.TamanoFuenteTitulo = 16f;
        config.TamanoFuenteTexto = 9f;
        config.TamanoFuenteTabla = 8f;
        config.ColorTitulo = "#27AE60";
        config.MargenSuperior = 40f;
        config.MargenInferior = 40f;
        return config;
    }

    private static ReporteConfiguration ConfiguracionCompacta(ReporteConfiguration config)
    {
        config.TamanoPagina = (int)TamanoPagina.A5;
        config.TamanoFuenteTitulo = 14f;
        config.TamanoFuenteTexto = 8f;
        config.TamanoFuenteTabla = 7f;
        config.MargenSuperior = 30f;
        config.MargenInferior = 30f;
        config.MargenIzquierdo = 30f;
        config.MargenDerecho = 30f;
        return config;
    }

    private static bool EsColorValido(string? color)
    {
        if (string.IsNullOrEmpty(color))
            return false;

        if (!color.StartsWith("#") || color.Length != 7)
            return false;

        try
        {
            Convert.ToInt32(color[1..], 16);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // public ReporteConfiguration? AplicarDefectosSiVacio(ReporteConfiguration? configuracionImpresion)
    // {
    //     throw new NotImplementedException();
    // }
}

// Enum para tipos de reporte
public enum TipoReporte
{
    Ejecutivo,
    Operativo,
    Tecnico,
    Compacto
}