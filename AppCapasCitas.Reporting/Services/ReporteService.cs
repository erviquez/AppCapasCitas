using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Configuration;
using AppCapasCitas.DTO.Request.Pago;
using AppCapasCitas.DTO.Request.Reporte;
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.Reporting.Helpers;
using AppCapasCitas.Transversal.Common;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AppCapasCitas.Reporting.Services;

public class ReporteService : IReporteService
{
    private readonly PdfAddDefaultSettings _pdfDefaults;
    private readonly IAsyncRepository<Paciente> _pacienteRepository;
    private readonly IAsyncRepository<Cita> _citaRepository;
    private readonly IAsyncRepository<Pago> _pagoRepository;
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<ReporteService> _logger;

    //Medicos
    private readonly IAsyncRepository<Medico> _medicoRepository;
    private readonly IAsyncRepository<HorarioTrabajo> _horarioTrabajoRepository;


    public ReporteService(
        PdfAddDefaultSettings pdfDefaults,
        IAsyncRepository<Paciente> pacienteRepository,
        IAsyncRepository<Cita> citaRepository,
        IAsyncRepository<Pago> pagoRepository,
        IAsyncRepository<Especialidad> especialidadRepository,
        IHostEnvironment hostEnvironment,
        ILogger<ReporteService> logger,
        IAsyncRepository<Medico> medicoRepository,
        IAsyncRepository<HorarioTrabajo> horarioTrabajoRepository

        )
    {
        _pdfDefaults = pdfDefaults;
        _pacienteRepository = pacienteRepository;
        _citaRepository = citaRepository;
        _pagoRepository = pagoRepository;
        _especialidadRepository = especialidadRepository;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
        _medicoRepository = medicoRepository;
        _horarioTrabajoRepository = horarioTrabajoRepository;
    }
    public async Task<Response<ReporteResponse>> GenerarReportePacientesAsync(ReporteRequest request)
    {
        var response = new Response<ReporteResponse>();

        try
        {
            // Usar helper para configuración
            var config = _pdfDefaults.AplicarConfiguracionDefault();
            // Validar configuración
            var errores = _pdfDefaults.ValidarConfiguracion(config!);
            if (errores.Any())
            {
                response.IsSuccess = false;
                response.Message = $"Errores de configuración: {string.Join(", ", errores)}";
                return response;
            }
            // Obtener datos
            var pacientes = await _pacienteRepository.GetAsync(
                predicate: p => (string.IsNullOrEmpty(request.FiltroNombre) ||
                               p.UsuarioNavigation!.Nombre!.Contains(request.FiltroNombre) ||
                               p.UsuarioNavigation!.Apellido!.Contains(request.FiltroNombre)) &&
                               (request.IncluirInactivos || p.Activo),
                orderBy: q => q.OrderBy(p => p.UsuarioNavigation!.Apellido).ThenBy(p => p.UsuarioNavigation!.Nombre),
                includes: new List<System.Linq.Expressions.Expression<Func<Paciente, object>>>
                {
                    p => p.UsuarioNavigation!
                },
                disableTracking: false,
                cancellationToken: default
            );

            if (request.FechaDesde.HasValue && request.FechaHasta.HasValue)
            {
                pacientes = pacientes.Where(p => p.FechaCreacion >= request.FechaDesde &&
                                               p.FechaCreacion <= request.FechaHasta).ToList();
            }

            // Generar PDF
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            using var pdfDoc = new PdfDocument(writer);
            var doc = new Document(pdfDoc);

            // Encabezado
            var logoPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "logo.png");
            PdfHelper.CrearEncabezado(doc, "Reporte de Pacientes", logoPath);

            // Información general
            var info = new Dictionary<string, string>
            {
                ["Fecha de Generación"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ["Total de Pacientes"] = pacientes.Count().ToString(),
                ["Filtro Aplicado"] = request.FiltroNombre ?? "Ninguno"
            };
            PdfHelper.CrearInformacionGeneral(doc, info);

            // Tabla de datos
            var headers = new List<string> { "Nombre Completo", "Email", "Teléfono", "Fecha Registro", "Estado" };
            var tabla = PdfHelper.CrearTabla(headers, pacientes.ToList(), p => new List<string>
            {
                $"{p.UsuarioNavigation!.Nombre} {p.UsuarioNavigation!.Apellido}",
                p.UsuarioNavigation!.Email ?? "",
                p.UsuarioNavigation!.Telefono ?? "",
                p.FechaCreacion.ToString("dd/MM/yyyy"),
                p.Activo ? "Activo" : "Inactivo"
            });

            doc.Add(tabla);

            // Pie de página
            PdfHelper.CrearPieDePagina(doc, $"Sistema de Gestión de Citas - Generado el {DateTime.Now:dd/MM/yyyy}");

            doc.Close();

            var ReporteResponse = new ReporteResponse
            {
                FileName = $"Reporte_Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                ContentType = "application/pdf",
                FileContent = ms.ToArray(),
                Base64Content = Convert.ToBase64String(ms.ToArray()),
                TotalRecords = pacientes.Count()
            };

            response.Data = ReporteResponse;
            response.IsSuccess = true;
            response.Message = "Reporte generado exitosamente";

            _logger.LogInformation($"Reporte de pacientes generado: {pacientes.Count()} registros");
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al generar reporte: {ex.Message}";
            _logger.LogError(ex, "Error al generar reporte de pacientes");
        }

        return response;
    }


    public async Task<Response<ReporteResponse>> GenerarReporteConfigurablePacientesAsync(ReporteRequest request)
    {
        var response = new Response<ReporteResponse>();

        try
        {
            var config = new ReporteConfiguration();
            // Configuración por defecto si no se proporciona
            if (request.ConfiguracionImpresion == null)
                config = _pdfDefaults.AplicarConfiguracionDefault();
            else
                config = request.ConfiguracionImpresion;
            // Obtener datos
            var pacientes = await _pacienteRepository.GetAsync(
            predicate: p => (string.IsNullOrEmpty(request.FiltroNombre) ||
                           p.UsuarioNavigation!.Nombre!.ToLower().Contains(request.FiltroNombre.ToLower()) ||
                           p.UsuarioNavigation!.Apellido!.ToLower().Contains(request.FiltroNombre.ToLower())) &&
                           (request.IncluirInactivos || p.Activo),
            orderBy: q => q.OrderBy(p => p.UsuarioNavigation!.Apellido).ThenBy(p => p.UsuarioNavigation!.Nombre),
            "UsuarioNavigation",
            disableTracking: false,
            cancellationToken: default
        );

            if (request.FechaDesde.HasValue && request.FechaHasta.HasValue)
            {
                pacientes = pacientes.Where(p => p.FechaCreacion >= request.FechaDesde &&
                                               p.FechaCreacion <= request.FechaHasta).ToList();
            }
            // Generar PDF con configuración
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);

            // using var pdfDoc = new PdfDocument(writer);
            // var doc = new Document(pdfDoc);          
            var doc = PdfConfigurableHelper.CrearDocumentoConfigurable(writer, config);
            var pdfDoc = doc.GetPdfDocument();


            // Encabezado
            //var logoPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images","logos", "logo.png");
            PdfConfigurableHelper.CrearEncabezadoConfigurable(doc, "Reporte de Pacientes", config);

            // Información general
            var info = new Dictionary<string, string>
            {
                ["Fecha de Generación"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ["Total de Pacientes"] = pacientes.Count().ToString(),
                ["Filtro Aplicado"] = request.FiltroNombre ?? "Ninguno"
            };
            PdfConfigurableHelper.CrearInformacionGeneralConfigurable(doc, info, config);

            // Tabla configurable
            var headers = new List<string> { "Nombre Completo", "Email", "Teléfono", "Celular", "Dirección", "Ciudad", "Estado", "Pais", "Fecha Registro", "Estado" };
            var tabla = PdfConfigurableHelper.CrearTablaConfigurable(headers, pacientes.ToList(), p => new List<string>
            {
                $"{p.UsuarioNavigation!.Nombre} {p.UsuarioNavigation!.Apellido}",
                p.UsuarioNavigation!.Email ?? "",
                p.UsuarioNavigation!.Telefono ?? "",
                p.UsuarioNavigation!.Celular ?? "",
                p.UsuarioNavigation!.Direccion ?? "",
                p.UsuarioNavigation!.Ciudad ?? "",
                p.UsuarioNavigation!.Estado ?? "",
                p.UsuarioNavigation!.Pais ?? "",
                p.FechaCreacion.ToString("dd/MM/yyyy"),
                p.Activo ? "Activo" : "Inactivo"
            }, config);

            doc.Add(tabla);


            // AHORA: Agregar imagen de fondo (después de cerrar el documento)
            // var imagenFondoPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "backgrounds", );
            var imagenFondoPath = Path.Combine(_hostEnvironment.ContentRootPath, config.ImagenFondoPath!);

            if (File.Exists(imagenFondoPath))
            {
                _logger.LogInformation("Agregando imagen de fondo...");
                PdfHelper.AgregarImagenFondo(pdfDoc, imagenFondoPath, config.OpacidadFondo);
            }
            else
            {
                _logger.LogWarning($"Imagen no encontrada, agregando marca de agua");
                PdfHelper.AgregarMarcaDeAgua(pdfDoc, config.TextoMarcaAgua!, config.OpacidadFondo);
            }
            // Pie de página configurable
            PdfConfigurableHelper.CrearPieDePaginaConfigurable(doc, config);
            // Cerrar PDF document
            doc.Close();
            pdfDoc.Close();

            var ReporteResponse = new ReporteResponse
            {
                FileName = $"Reporte_Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                ContentType = "application/pdf",
                FileContent = ms.ToArray(),
                Base64Content = Convert.ToBase64String(ms.ToArray()),
                TotalRecords = pacientes.Count()
            };

            response.Data = ReporteResponse;
            response.IsSuccess = true;
            response.Message = "Reporte generado exitosamente";

            _logger.LogInformation($"Reporte de pacientes generado: {pacientes.Count()} registros");
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al generar reporte: {ex.Message}";
            _logger.LogError(ex, "Error al generar reporte de pacientes");
        }

        return response;
    }

    public async Task<Response<ReporteResponse>> GenerarReporteConfigurableMedicosAsync(ReporteRequest request)
    {
        var response = new Response<ReporteResponse>();

        try
        {
            var config = new ReporteConfiguration();
            // Configuración por defecto si no se proporciona
            if (request.ConfiguracionImpresion == null)
                config = _pdfDefaults.AplicarConfiguracionDefault();
            else
                config = request.ConfiguracionImpresion;
            // Obtener datos
            //si filtro nombre es vacio
 
            var medicos = await _medicoRepository.GetAsync(
            predicate: p => (string.IsNullOrEmpty(request.FiltroNombre) ||
                           p.UsuarioNavigation!.Nombre!.ToLower().Contains(request.FiltroNombre.ToLower()) ||
                           p.UsuarioNavigation!.Apellido!.ToLower().Contains(request.FiltroNombre.ToLower())) &&
                           (request.IncluirInactivos || p.Activo),
            orderBy: q => q.OrderBy(p => p.UsuarioNavigation!.Apellido).ThenBy(p => p.UsuarioNavigation!.Nombre),
            "UsuarioNavigation",
            disableTracking: false,
            cancellationToken: default
        );

            if (request.FechaDesde.HasValue && request.FechaHasta.HasValue)
            {
                medicos = medicos.Where(p => p.FechaCreacion >= request.FechaDesde &&
                                               p.FechaCreacion <= request.FechaHasta).ToList();
            }
            // Generar PDF con configuración
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var doc = PdfConfigurableHelper.CrearDocumentoConfigurable(writer, config);
            var pdfDoc = doc.GetPdfDocument();


            // Encabezado
            PdfConfigurableHelper.CrearEncabezadoConfigurable(doc, "Reporte de Medicos", config);

            // Información general
            var info = new Dictionary<string, string>
            {
                ["Fecha de Generación"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ["Total de Medicos"] = medicos.Count().ToString(),
                ["Filtro Aplicado"] = request.FiltroNombre ?? "Ninguno"
            };
            PdfConfigurableHelper.CrearInformacionGeneralConfigurable(doc, info, config);

            // Tabla configurable
            var headers = new List<string> { "Nombre Completo", "Email", "Teléfono", "Celular", "Dirección", "Ciudad", "Estado", "Pais", "Fecha Registro", "Estado" };
            var tabla = PdfConfigurableHelper.CrearTablaConfigurable(headers, medicos.ToList(), p => new List<string>
            {
                $"{p.UsuarioNavigation!.Nombre} {p.UsuarioNavigation!.Apellido}",
                p.UsuarioNavigation!.Email ?? "",
                p.UsuarioNavigation!.Telefono ?? "",
                p.UsuarioNavigation!.Celular ?? "",
                p.UsuarioNavigation!.Direccion ?? "",
                p.UsuarioNavigation!.Ciudad ?? "",
                p.UsuarioNavigation!.Estado ?? "",
                p.UsuarioNavigation!.Pais ?? "",
                p.FechaCreacion.ToString("dd/MM/yyyy"),
                p.Activo ? "Activo" : "Inactivo"
            }, config);

            doc.Add(tabla);

            var imagenFondoPath = Path.Combine(_hostEnvironment.ContentRootPath, config.ImagenFondoPath!);

            if (File.Exists(imagenFondoPath))
            {
                _logger.LogInformation("Agregando imagen de fondo...");
                PdfHelper.AgregarImagenFondo(pdfDoc, imagenFondoPath, config.OpacidadFondo);
            }
            else
            {
                _logger.LogWarning($"Imagen no encontrada, agregando marca de agua");
                PdfHelper.AgregarMarcaDeAgua(pdfDoc, config.TextoMarcaAgua!, config.OpacidadFondo);
            }
            // Pie de página configurable
            PdfConfigurableHelper.CrearPieDePaginaConfigurable(doc, config);
            // Cerrar PDF document
            doc.Close();
            pdfDoc.Close();

            var ReporteResponse = new ReporteResponse
            {
                FileName = $"Reporte_Medicos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                ContentType = "application/pdf",
                FileContent = ms.ToArray(),
                Base64Content = Convert.ToBase64String(ms.ToArray()),
                TotalRecords = medicos.Count()
            };

            response.Data = ReporteResponse;
            response.IsSuccess = true;
            response.Message = "Reporte generado exitosamente";

            _logger.LogInformation($"Reporte de medicos generado: {medicos.Count()} registros");
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al generar reporte: {ex.Message}";
            _logger.LogError(ex, "Error al generar reporte de medicos");
        }

        return response;
    }
    public Task<Response<ReporteResponse>> GenerarReporteCitasAsync(ReporteCitaRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<ReporteResponse>> GenerarReportePagosAsync(ReportePagoRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<ReporteResponse>> GenerarReporteEspecialidadesAsync()
    {
        throw new NotImplementedException();
    }



    ////////////////////////
    public async Task<Response<ReporteResponse>> GenerarExpedienteMedicoAsync(ReporteIdRequest request)
    {
        var response = new Response<ReporteResponse>();

        try
        {
            var config = new ReporteConfiguration();
            // Configuración por defecto si no se proporciona
            if (request.ConfiguracionImpresion == null)
                config = _pdfDefaults.AplicarConfiguracionDefault();
            else
                config = request.ConfiguracionImpresion;

            // Obtener datos del médico con todas las relaciones
            var medico = await _medicoRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                includes: new List<System.Linq.Expressions.Expression<Func<Medico, object>>>
                {
                    m => m.UsuarioNavigation!,
                    m => m.MedicoEspecialidadHospitales!,
                    m => m.HorariosTrabajo!,
                    m => m.Citas!
                },
                disableTracking: false,
                cancellationToken: default
            );

            if (!medico.Any())
            {
                response.IsSuccess = false;
                response.Message = "Médico no encontrado";
                return response;
            }

            var medicoData = medico.First();

            // Generar PDF con configuración
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var doc = PdfConfigurableHelper.CrearDocumentoConfigurable(writer, config);
            var pdfDoc = doc.GetPdfDocument();

            // Agregar imagen de fondo
            var imagenFondoPath = Path.Combine(_hostEnvironment.ContentRootPath, config.ImagenFondoPath!);
            if (File.Exists(imagenFondoPath))
            {
                PdfHelper.AgregarImagenFondo(pdfDoc, imagenFondoPath, config.OpacidadFondo);
            }

            // ENCABEZADO TIPO EXPEDIENTE
            PdfConfigurableHelper.CrearEncabezadoExpediente(doc, "EXPEDIENTE DEL MÉDICO", config);

            // INFORMACIÓN PERSONAL
            CrearSeccionInformacionPersonal(doc, medicoData, config);

            // INFORMACIÓN PROFESIONAL
            CrearSeccionInformacionProfesional(doc, medicoData, config);

            // ESPECIALIDADES Y HOSPITALES
            CrearSeccionEspecialidadesHospitales(doc, medicoData, config);

            // HORARIOS DE TRABAJO
            CrearSeccionHorariosTrabajo(doc, medicoData, config);

            // ESTADÍSTICAS
            await CrearSeccionEstadisticas(doc, medicoData, config);

            // Pie de página
            PdfConfigurableHelper.CrearPieDePaginaConfigurable(doc, config);

            doc.Close();
            pdfDoc.Close();

            var ReporteResponse = new ReporteResponse
            {
                FileName = $"Expediente_Medico_{medicoData.UsuarioNavigation?.Apellido}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                ContentType = "application/pdf",
                FileContent = ms.ToArray(),
                Base64Content = Convert.ToBase64String(ms.ToArray()),
                TotalRecords = 1
            };

            response.Data = ReporteResponse;
            response.IsSuccess = true;
            response.Message = "Expediente generado exitosamente";

            _logger.LogInformation($"Expediente médico generado para: {medicoData.UsuarioNavigation?.Nombre} {medicoData.UsuarioNavigation?.Apellido}");
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al generar expediente: {ex.Message}";
            _logger.LogError(ex, "Error al generar expediente médico");
        }

        return response;
    }

    #region Métodos Auxiliares para Expediente

    private void CrearSeccionInformacionPersonal(Document doc, Medico medico, ReporteConfiguration config)
    {
        // Título de sección
        PdfConfigurableHelper.CrearTituloSeccion(doc, "INFORMACIÓN PERSONAL", config);

        // Crear layout de dos columnas
        var tabla = new Table(new float[] { 1, 1 });
        tabla.SetWidth(UnitValue.CreatePercentValue(100));

        // Columna izquierda
        var columnaIzq = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetPadding(10);

        columnaIzq.Add(CrearCampoExpediente("Nombre Completo:", 
            $"{medico.UsuarioNavigation?.Nombre} {medico.UsuarioNavigation?.Apellido}", config));
        columnaIzq.Add(CrearCampoExpediente("Email:", medico.UsuarioNavigation?.Email ?? "N/A", config));
        columnaIzq.Add(CrearCampoExpediente("Teléfono:", medico.UsuarioNavigation?.Telefono ?? "N/A", config));
        columnaIzq.Add(CrearCampoExpediente("Celular:", medico.UsuarioNavigation?.Celular ?? "N/A", config));
        columnaIzq.Add(CrearCampoExpediente("Género:", medico.UsuarioNavigation?.Genero ?? "N/A", config));

        // Columna derecha
        var columnaDer = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetPadding(10);

        columnaDer.Add(CrearCampoExpediente("Dirección:", medico.UsuarioNavigation?.Direccion ?? "N/A", config));
        columnaDer.Add(CrearCampoExpediente("Ciudad:", medico.UsuarioNavigation?.Ciudad ?? "N/A", config));
        columnaDer.Add(CrearCampoExpediente("Estado:", medico.UsuarioNavigation?.Estado ?? "N/A", config));
        columnaDer.Add(CrearCampoExpediente("País:", medico.UsuarioNavigation?.Pais ?? "N/A", config));
        columnaDer.Add(CrearCampoExpediente("Fecha de Registro:", medico.FechaCreacion.ToString("dd/MM/yyyy"), config));

        tabla.AddCell(columnaIzq);
        tabla.AddCell(columnaDer);
        doc.Add(tabla);
        doc.Add(new Paragraph("\n"));
    }

    private void CrearSeccionInformacionProfesional(Document doc, Medico medico, ReporteConfiguration config)
    {
        // Título de sección
        PdfConfigurableHelper.CrearTituloSeccion(doc, "INFORMACIÓN PROFESIONAL", config);

        var tabla = new Table(new float[] { 1, 1 });
        tabla.SetWidth(UnitValue.CreatePercentValue(100));

        // Columna izquierda
        var columnaIzq = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetPadding(10);

        columnaIzq.Add(CrearCampoExpediente("Universidad:", medico.Universidad ?? "N/A", config));
        columnaIzq.Add(CrearCampoExpediente("Cédula Profesional:", medico.CedulaProfesional ?? "N/A", config));
        columnaIzq.Add(CrearCampoExpediente("Años de Experiencia:", "10" ?? "N/A", config));
        //columnaIzq.Add(CrearCampoExpediente("Años de Experiencia:", medico.AniosExperiencia?.ToString() ?? "N/A", config));

        // Columna derecha
        var columnaDer = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetPadding(10);

        columnaDer.Add(CrearCampoExpediente("Estado:", medico.Activo ? "Activo" : "Inactivo", config));
        columnaDer.Add(CrearCampoExpediente("Último Login:", 
            medico.UsuarioNavigation?.UltimoLogin?.ToString("dd/MM/yyyy HH:mm") ?? "N/A", config));

        tabla.AddCell(columnaIzq);
        tabla.AddCell(columnaDer);
        doc.Add(tabla);

        // Biografía (campo largo)
        if (!string.IsNullOrEmpty(medico.Biografia))
        {
            doc.Add(CrearCampoExpediente("Biografía:", medico.Biografia, config));
        }

        doc.Add(new Paragraph("\n"));
    }

    private IBlockElement CrearCampoExpediente(string v, object value, ReporteConfiguration config)
    {
        throw new NotImplementedException();
    }

    private void CrearSeccionEspecialidadesHospitales(Document doc, Medico medico, ReporteConfiguration config)
    {
        PdfConfigurableHelper.CrearTituloSeccion(doc, "ESPECIALIDADES Y HOSPITALES", config);

        if (medico.MedicoEspecialidadHospitales?.Any() == true)
        {
            var headers = new List<string> { "Especialidad", "Hospital", "Cargo", "Estado" };
            var tabla = PdfConfigurableHelper.CrearTablaConfigurable(headers, 
                medico.MedicoEspecialidadHospitales.ToList(), 
                item => new List<string>
                {
                    item.EspecialidadNavigation?.Nombre ?? "N/A",
                    item.HospitalNavigation?.Nombre ?? "N/A",
                    item.CargoNavigation?.Nombre ?? "N/A",
                    item.Activo ? "Activo" : "Inactivo"
                }, config);
            
            doc.Add(tabla);
        }
        else
        {
            doc.Add(new Paragraph("No hay especialidades registradas")
                .SetFontSize(config.TamanoFuenteTexto)
                .SetItalic());
        }

        doc.Add(new Paragraph("\n"));
    }

    private void CrearSeccionHorariosTrabajo(Document doc, Medico medico, ReporteConfiguration config)
    {
        PdfConfigurableHelper.CrearTituloSeccion(doc, "HORARIOS DE TRABAJO", config);

        if (medico.HorariosTrabajo?.Any() == true)
        {
            var headers = new List<string> { "Día", "Hora Inicio", "Hora Fin", "Estado" };
            var tabla = PdfConfigurableHelper.CrearTablaConfigurable(headers, 
                medico.HorariosTrabajo.OrderBy(h => h.DiaSemana).ToList(), 
                horario => new List<string>
                {
                    ObtenerNombreDia((int)horario.DiaSemana),
                    (horario.HoraInicio != null ? horario.HoraInicio.ToString(@"hh\:mm") : "N/A"),
                    (horario.HoraFin != null ? horario.HoraFin.ToString(@"hh\:mm") : "N/A"),
                    horario.Activo ? "Activo" : "Inactivo"
                }, config);
            
            doc.Add(tabla);
        }
        else
        {
            doc.Add(new Paragraph("No hay horarios registrados")
                .SetFontSize(config.TamanoFuenteTexto)
                .SetItalic());
        }

        doc.Add(new Paragraph("\n"));
    }

    private async Task CrearSeccionEstadisticas(Document doc, Medico medico, ReporteConfiguration config)
    {
        PdfConfigurableHelper.CrearTituloSeccion(doc, "ESTADÍSTICAS", config);

        // Obtener estadísticas del médico
        var totalCitas = medico.Citas?.Count() ?? 0;
        var citasCompletadas = medico.Citas?.Count(c => c.Estado == "Completada") ?? 0;
        var citasPendientes = medico.Citas?.Count(c => c.Estado == "Programada") ?? 0;
        var citasCanceladas = medico.Citas?.Count(c => c.Estado == "Cancelada") ?? 0;

        var estadisticas = new Dictionary<string, string>
        {
            ["Total de Citas"] = totalCitas.ToString(),
            ["Citas Completadas"] = citasCompletadas.ToString(),
            ["Citas Pendientes"] = citasPendientes.ToString(),
            ["Citas Canceladas"] = citasCanceladas.ToString(),
            ["Porcentaje de Completadas"] = totalCitas > 0 ? $"{(citasCompletadas * 100.0 / totalCitas):F1}%" : "0%"
        };

        var tabla = new Table(new float[] { 1, 1 });
        tabla.SetWidth(UnitValue.CreatePercentValue(100));

        foreach (var stat in estadisticas)
        {
            tabla.AddCell(new Cell()
                .Add(new Paragraph(stat.Key)
                    .SetFontSize(config.TamanoFuenteTexto)
                    .SetBold())
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            
            tabla.AddCell(new Cell()
                .Add(new Paragraph(stat.Value)
                    .SetFontSize(config.TamanoFuenteTexto))
                .SetTextAlignment(TextAlignment.CENTER));
        }

        doc.Add(tabla);
    }

    private Paragraph CrearCampoExpediente(string etiqueta, string valor, ReporteConfiguration config)
    {
        return new Paragraph()
            .Add(new Text(etiqueta + " ").SetBold().SetFontSize(config.TamanoFuenteTexto))
            .Add(new Text(valor).SetFontSize(config.TamanoFuenteTexto))
            .SetMarginBottom(5);
    }

    private string ObtenerNombreDia(int diaSemana)
    {
        return diaSemana switch
        {
            1 => "Lunes",
            2 => "Martes", 
            3 => "Miércoles",
            4 => "Jueves",
            5 => "Viernes",
            6 => "Sábado",
            7 => "Domingo",
            _ => "N/A"
        };
    }

    #endregion
    /////////////////// 

}