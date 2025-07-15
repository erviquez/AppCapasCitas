
using AppCapasCitas.DTO.Configuration;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Globalization;

namespace AppCapasCitas.Reporting.Helpers;

public static class PdfConfigurableHelper
{
    public static Document CrearDocumentoConfigurable(PdfWriter writer, ReporteConfiguration? config = null)
    {
        config ??= new ReporteConfiguration();

        var pageSize = ObtenerTamanoPagina((TamanoPagina)config.TamanoPagina, (OrientacionPagina)config.Orientacion);
        var pdfDoc = new PdfDocument(writer);
        var doc = new Document(pdfDoc, pageSize);

        // Configurar márgenes
        doc.SetMargins(config.MargenSuperior, config.MargenDerecho,
                      config.MargenInferior, config.MargenIzquierdo);

        // Agregar imagen de fondo si existe
        if (!string.IsNullOrEmpty(config.ImagenFondoBase64))
        {
            AgregarImagenFondo(pdfDoc, config.ImagenFondoBase64, config.OpacidadFondo);
        }

        return doc;
    }

    public static PageSize ObtenerTamanoPagina(TamanoPagina tamaño, OrientacionPagina orientacion)
    {
        PageSize pageSize = tamaño switch
        {
            TamanoPagina.A3 => PageSize.A3,
            TamanoPagina.A4 => PageSize.A4,
            TamanoPagina.A5 => PageSize.A5,
            TamanoPagina.Letter => PageSize.LETTER,
            TamanoPagina.Legal => PageSize.LEGAL,
            TamanoPagina.Tabloid => PageSize.TABLOID,
            _ => PageSize.A4
        };

        return orientacion == OrientacionPagina.Horizontal ? pageSize.Rotate() : pageSize;
    }

    public static PdfFont ObtenerFuente(string tipoFuente)
    {
        return tipoFuente.ToLower() switch
        {
            "times" or "times new roman" => PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN),
            "courier" => PdfFontFactory.CreateFont(StandardFonts.COURIER),
            "helvetica" => PdfFontFactory.CreateFont(StandardFonts.HELVETICA),
            _ => PdfFontFactory.CreateFont(StandardFonts.HELVETICA) // Arial no está en StandardFonts, usar Helvetica
        };
    }

    public static Color ObtenerColor(string colorHex)
    {
        try
        {
            if (colorHex.StartsWith("#"))
                colorHex = colorHex[1..];

            var r = Convert.ToInt32(colorHex[0..2], 16);
            var g = Convert.ToInt32(colorHex[2..4], 16);
            var b = Convert.ToInt32(colorHex[4..6], 16);

            return new DeviceRgb(r, g, b);
        }
        catch
        {
            return ColorConstants.BLACK;
        }
    }

    public static void CrearEncabezadoConfigurable(Document doc, string titulo, ReporteConfiguration config)
    {
        var headerTable = new Table(2);
        headerTable.SetWidth(UnitValue.CreatePercentValue(100));

        // Logo
        if (!string.IsNullOrEmpty(config.LogoBase64))
        {
            try
            {
                var logoBytes = Convert.FromBase64String(config.LogoBase64);
                var logo = new Image(ImageDataFactory.Create(logoBytes));
                logo.SetHeight(50);
                headerTable.AddCell(new Cell().Add(logo).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            }
            catch
            {
                headerTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            }
        }
        else
        {
            headerTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
        }

        // Título
        var color = ObtenerColor(config.ColorTitulo);

        var titleCell = new Cell()
            .Add(new Paragraph(titulo)
                .SetFontSize(config.TamanoFuenteTitulo)
                .SetFontColor(color)
                .SetBold()
                .SetTextAlignment(TextAlignment.RIGHT))
            .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

        headerTable.AddCell(titleCell);
        doc.Add(headerTable);
        doc.Add(new Paragraph("\n"));
    }

    public static void CrearInformacionGeneralConfigurable(Document doc, Dictionary<string, string> info, ReporteConfiguration config)
    {
        var font = ObtenerFuente(config.TipoFuente);
        var color = ObtenerColor(config.ColorTexto);

        var infoTable = new Table(2);
        infoTable.SetWidth(UnitValue.CreatePercentValue(50));

        foreach (var item in info)
        {
            infoTable.AddCell(new Cell()
                .Add(new Paragraph(item.Key + ":")
                    .SetFont(font)
                    .SetFontSize(config.TamanoFuenteTexto)
                    .SetFontColor(color)
                    .SetBold()));

            infoTable.AddCell(new Cell()
                .Add(new Paragraph(item.Value)
                    .SetFont(font)
                    .SetFontSize(config.TamanoFuenteTexto)
                    .SetFontColor(color)));
        }

        doc.Add(infoTable);
        doc.Add(new Paragraph("\n"));
    }

    public static Table CrearTablaConfigurable<T>(
        List<string> headers,
        List<T> data,
        Func<T, List<string>> dataExtractor,
        ReporteConfiguration config,
        List<float>? columnWidths = null)
    {
        var table = new Table(headers.Count);
        var font = ObtenerFuente(config.TipoFuente);
        var colorTexto = ObtenerColor(config.ColorTexto);
        var colorEncabezado = ObtenerColor(config.ColorEncabezadoTabla);

        if (columnWidths != null && columnWidths.Count == headers.Count)
        {
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetFixedLayout();
        }

        // Agregar encabezados
        foreach (var header in headers)
        {
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph(header)
                    .SetFont(font)
                    .SetFontSize(config.TamanoFuenteTabla)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBold())
                .SetBackgroundColor(colorEncabezado)
                .SetTextAlignment(TextAlignment.CENTER));
        }

        // Agregar datos
        foreach (var item in data)
        {
            var rowData = dataExtractor(item);
            foreach (var cellData in rowData)
            {
                table.AddCell(new Cell()
                    .Add(new Paragraph(cellData)
                        .SetFont(font)
                        .SetFontSize(config.TamanoFuenteTabla)
                        .SetFontColor(colorTexto)));
            }
        }

        return table;
    }

    public static void CrearPieDePaginaConfigurable(Document doc, ReporteConfiguration config)
    {
        if (!config.MostrarFechaGeneracion && !config.MostrarNumeroPagina && string.IsNullOrEmpty(config.PiePagina))
            return;

        var font = ObtenerFuente(config.TipoFuente);
        var color = ObtenerColor(config.ColorTexto);

        doc.Add(new Paragraph("\n"));

        var piePaginaTexto = "";

        if (!string.IsNullOrEmpty(config.PiePagina))
        {
            piePaginaTexto += config.PiePagina;
        }

        if (config.MostrarFechaGeneracion)
        {
            if (!string.IsNullOrEmpty(piePaginaTexto)) piePaginaTexto += " - ";
            piePaginaTexto += $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
        }

        if (!string.IsNullOrEmpty(piePaginaTexto))
        {
            doc.Add(new Paragraph(piePaginaTexto)
                .SetFont(font)
                .SetFontSize(config.TamanoFuenteTexto - 1)
                .SetFontColor(color)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetItalic());
        }
    }

    private static void AgregarImagenFondo(PdfDocument pdfDoc, string imagenBase64, float opacidad)
    {
        try
        {
            var imageBytes = Convert.FromBase64String(imagenBase64);
            var imageData = ImageDataFactory.Create(imageBytes);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                var pageSize = page.GetPageSize();
                var image = new Image(imageData);

                // Escalar imagen para ajustar a la página
                image.ScaleToFit(pageSize.GetWidth(), pageSize.GetHeight());
                image.SetFixedPosition(0, 0);

                // Aplicar opacidad
                canvas.SaveState();
                canvas.SetFillColorGray(opacidad);
                // canvas.AddImage(imageData, pageSize.GetWidth(), 0, 0, pageSize.GetHeight(), 0, 0);
                canvas.RestoreState();
            }
        }
        catch (Exception ex)
        {
            // Log error but continue without background image
            System.Diagnostics.Debug.WriteLine($"Error al agregar imagen de fondo: {ex.Message}");
        }
    }
    

    public static void CrearEncabezadoExpediente(Document doc, string titulo, ReporteConfiguration config)
    {
        // Línea superior decorativa
        var linea = new SolidLine(2f);
        var lineaSeparador = new LineSeparator(linea);
        lineaSeparador.SetWidth(UnitValue.CreatePercentValue(100));
        lineaSeparador.SetMarginBottom(10);
        doc.Add(lineaSeparador);

        // Título principal centrado
        var tituloParrafo = new Paragraph(titulo)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(config.TamanoFuenteTitulo)
            .SetBold()
            .SetFontColor(new DeviceRgb(
                int.Parse(config.ColorTitulo.Substring(1, 2), NumberStyles.HexNumber),
                int.Parse(config.ColorTitulo.Substring(3, 2), NumberStyles.HexNumber), 
                int.Parse(config.ColorTitulo.Substring(5, 2), NumberStyles.HexNumber)))
            .SetMarginBottom(10);
        
        doc.Add(tituloParrafo);

        // Información del header
        var fechaGeneracion = new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontSize(config.TamanoFuenteTexto)
            .SetMarginBottom(15);
        
        doc.Add(fechaGeneracion);
        
        // Línea inferior
        doc.Add(lineaSeparador);
    }

    public static void CrearTituloSeccion(Document doc, string titulo, ReporteConfiguration config)
    {
        var tituloSeccion = new Paragraph(titulo)
            .SetFontSize(config.TamanoFuenteSubtitulo)
            .SetBold()
            .SetFontColor(new DeviceRgb(
                int.Parse(config.ColorTitulo.Substring(1, 2), NumberStyles.HexNumber),
                int.Parse(config.ColorTitulo.Substring(3, 2), NumberStyles.HexNumber), 
                int.Parse(config.ColorTitulo.Substring(5, 2), NumberStyles.HexNumber)))
            .SetMarginTop(15)
            .SetMarginBottom(10)
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetPadding(5);
        
        doc.Add(tituloSeccion);
    }
}