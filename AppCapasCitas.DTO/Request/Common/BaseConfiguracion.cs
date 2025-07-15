using System;
using AppCapasCitas.DTO.Configuration;
using AppCapasCitas.DTO.Request.Reporte;

namespace AppCapasCitas.DTO.Request;

public abstract class BasePaginaConfiguracion
{
    //Configuración de impresión
    public ReporteConfiguration? ConfiguracionImpresion { get; set; }
}
