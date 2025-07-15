using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;

namespace AppCapasCitas.Reporting.Helpers;

public static class PdfHelper
{
    public static void CrearEncabezado(Document doc, string titulo, string logoPath = "")
    {
        var headerTable = new Table(2);
        headerTable.SetWidth(UnitValue.CreatePercentValue(100));

        // Logo (si existe)
        if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
        {
            var logo = new Image(ImageDataFactory.Create(logoPath));
            logo.SetHeight(50);
            headerTable.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER));
        }
        else
        {
            headerTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
        }

        // Título
        var titleCell = new Cell()
            .Add(new Paragraph(titulo)
                .SetFontSize(18)
                .SetBold()
                .SetTextAlignment(TextAlignment.RIGHT))
            .SetBorder(Border.NO_BORDER);

        headerTable.AddCell(titleCell);
        doc.Add(headerTable);
        doc.Add(new Paragraph("\n"));
    }

    public static void CrearInformacionGeneral(Document doc, Dictionary<string, string> info)
    {
        var infoTable = new Table(2);
        infoTable.SetWidth(UnitValue.CreatePercentValue(50));

        foreach (var item in info)
        {
            infoTable.AddCell(new Cell().Add(new Paragraph(item.Key + ":").SetBold()));
            infoTable.AddCell(new Cell().Add(new Paragraph(item.Value)));
        }

        doc.Add(infoTable);
        doc.Add(new Paragraph("\n"));
    }

    public static Table CrearTabla<T>(List<string> headers, List<T> data,
        Func<T, List<string>> dataExtractor, List<float>? columnWidths = null)
    {
        var table = new Table(headers.Count);

        if (columnWidths != null && columnWidths.Count == headers.Count)
        {
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetFixedLayout();
        }

        // Agregar encabezados
        foreach (var header in headers)
        {
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph(header).SetBold())
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
        }

        // Agregar datos
        foreach (var item in data)
        {
            var rowData = dataExtractor(item);
            foreach (var cellData in rowData)
            {
                table.AddCell(new Cell().Add(new Paragraph(cellData)));
            }
        }

        return table;
    }

    public static void CrearPieDePagina(Document doc, string texto)
    {
        doc.Add(new Paragraph("\n"));
        doc.Add(new Paragraph(texto)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(10)
            .SetItalic());
    }



    /// <summary>
    /// Agrega una imagen de fondo a todas las páginas del documento
    /// </summary>
    public static void AgregarImagenFondo(PdfDocument pdfDoc, string imagePath, float opacity = 0.1f)
    {
        if (!File.Exists(imagePath))
        {
            Console.WriteLine($"Imagen no encontrada: {imagePath}");
            return;
        }

        try
        {
            var imageData = ImageDataFactory.Create(imagePath);

            // Agregar imagen de fondo a todas las páginas existentes
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                //AgregarImagenFondoAPagina(pdfDoc, i, imageData, opacity);
                AgregarImagenFondoAPaginaCorregida(pdfDoc, i, imageData, opacity);
            }

            // Agregar evento para páginas nuevas que se creen después
            pdfDoc.AddEventHandler(iText.Kernel.Events.PdfDocumentEvent.END_PAGE,
                new ImagenFondoEventHandler(imageData, opacity));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar imagen de fondo: {ex.Message}");
        }
    }

    private static void AgregarImagenFondoAPagina(PdfDocument pdfDoc, int pageNumber, ImageData imageData, float opacity)
    {
        try
        {
            var page = pdfDoc.GetPage(pageNumber);
            var pageSize = page.GetPageSize();
            var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            // Configurar opacidad
            var gs = new iText.Kernel.Pdf.Extgstate.PdfExtGState().SetFillOpacity(opacity);
            canvas.SetExtGState(gs);

            // Calcular dimensiones para cubrir toda la página manteniendo proporción
            var imageWidth = imageData.GetWidth();
            var imageHeight = imageData.GetHeight();
            var pageWidth = pageSize.GetWidth();
            var pageHeight = pageSize.GetHeight();

            // Escalar para cubrir toda la página
            var scaleX = pageWidth / imageWidth;
            var scaleY = pageHeight / imageHeight;
            var scale = Math.Max(scaleX, scaleY); // Usar Max para cubrir completamente

            var scaledWidth = imageWidth * scale;
            var scaledHeight = imageHeight * scale;

            // Centrar la imagen
            var x = (pageWidth - scaledWidth) / 2;
            var y = (pageHeight - scaledHeight) / 2;

            // Agregar imagen usando AddImageFittedIntoRectangle para escalar y centrar
            var rect = new iText.Kernel.Geom.Rectangle(x, y, scaledWidth, scaledHeight);
            canvas.AddImageFittedIntoRectangle(imageData, rect, false);
            canvas.Release();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar imagen a la página {pageNumber}: {ex.Message}");
        }
    }
    private static void AgregarImagenFondoAPaginaCorregida(PdfDocument pdfDoc, int pageNumber, ImageData imageData, float opacity)
    {
        try
        {
            var page = pdfDoc.GetPage(pageNumber);
            var pageSize = page.GetPageSize();
            
            // IMPORTANTE: Usar NewContentStreamBefore() para que esté en el fondo
            var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            // GUARDAR estado actual del canvas
            canvas.SaveState();

            // Configurar opacidad SOLO para esta operación
            var gs = new iText.Kernel.Pdf.Extgstate.PdfExtGState().SetFillOpacity(opacity);
            canvas.SetExtGState(gs);

            // Calcular dimensiones para cubrir toda la página manteniendo proporción
            var imageWidth = imageData.GetWidth();
            var imageHeight = imageData.GetHeight();
            var pageWidth = pageSize.GetWidth();
            var pageHeight = pageSize.GetHeight();

            // Escalar para cubrir toda la página
            var scaleX = pageWidth / imageWidth;
            var scaleY = pageHeight / imageHeight;
            var scale = Math.Max(scaleX, scaleY);

            var scaledWidth = imageWidth * scale;
            var scaledHeight = imageHeight * scale;

            // Centrar la imagen
            var x = (pageWidth - scaledWidth) / 2;
            var y = (pageHeight - scaledHeight) / 2;

            // Agregar imagen
            var rect = new iText.Kernel.Geom.Rectangle(x, y, scaledWidth, scaledHeight);
            canvas.AddImageFittedIntoRectangle(imageData, rect, false);
            
            // RESTAURAR estado del canvas - ESTO ES CLAVE
            canvas.RestoreState();
            canvas.Release();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar imagen a la página {pageNumber}: {ex.Message}");
        }
    }
    /// <summary>
    /// Agrega marca de agua de texto
    /// </summary>
    public static void AgregarMarcaDeAgua(PdfDocument pdfDoc, string watermarkText, float opacity = 0.3f)
    {
        try
        {
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var pageSize = page.GetPageSize();
                var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                // Configurar opacidad y color
                var gs = new iText.Kernel.Pdf.Extgstate.PdfExtGState().SetFillOpacity(opacity);
                canvas.SetExtGState(gs);
                canvas.SetFillColor(ColorConstants.LIGHT_GRAY);

                // Calcular posición centrada con rotación
                var centerX = pageSize.GetWidth() / 2;
                var centerY = pageSize.GetHeight() / 2;

                // Agregar texto rotado en diagonal
                canvas.BeginText()
                      .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(), 50)
                      .SetTextMatrix((float)Math.Cos(Math.PI / 4), (float)Math.Sin(Math.PI / 4),
                                   -(float)Math.Sin(Math.PI / 4), (float)Math.Cos(Math.PI / 4),
                                   centerX - 100, centerY)
                      .ShowText(watermarkText)
                      .EndText();

                canvas.Release();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar marca de agua: {ex.Message}");
        }
    }
}

// Event handler para agregar imagen de fondo a páginas nuevas
public class ImagenFondoEventHandler : iText.Kernel.Events.IEventHandler
{
    private readonly ImageData _imageData;
    private readonly float _opacity;

    public ImagenFondoEventHandler(ImageData imageData, float opacity)
    {
        _imageData = imageData;
        _opacity = opacity;
    }

    public void HandleEvent(iText.Kernel.Events.Event currentEvent)
    {
        var docEvent = (iText.Kernel.Events.PdfDocumentEvent)currentEvent;
        var pdfDoc = docEvent.GetDocument();
        var page = docEvent.GetPage();
        
        try
        {
            var pageSize = page.GetPageSize();
            var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            
            var gs = new iText.Kernel.Pdf.Extgstate.PdfExtGState().SetFillOpacity(_opacity);
            canvas.SetExtGState(gs);
            
            var imageWidth = _imageData.GetWidth();
            var imageHeight = _imageData.GetHeight();
            var pageWidth = pageSize.GetWidth();
            var pageHeight = pageSize.GetHeight();
            
            var scaleX = pageWidth / imageWidth;
            var scaleY = pageHeight / imageHeight;
            var scale = Math.Max(scaleX, scaleY);
            
            var scaledWidth = imageWidth * scale;
            var scaledHeight = imageHeight * scale;
            
            var x = (pageWidth - scaledWidth) / 2;
            var y = (pageHeight - scaledHeight) / 2;
            var rect = new iText.Kernel.Geom.Rectangle(x, y, scaledWidth, scaledHeight);
            canvas.AddImageFittedIntoRectangle(_imageData, rect, false);
            canvas.Release();
            canvas.Release();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en event handler: {ex.Message}");
        }
    }
}