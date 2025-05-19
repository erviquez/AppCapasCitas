using Microsoft.Extensions.Logging;


namespace AppCapasCitas.Transversal.Logging;

public class HtmlFileLogger : ILogger
{
    private readonly string _basePath;
    private readonly string _categoryName;

    public HtmlFileLogger(string basePath, string categoryName)
    {
        _basePath = basePath;
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var filePath = _basePath.Replace(".html", $"-{today}.html");
        InitializeStyles(filePath);
        var message = formatter(state, exception);
        var logEntry = FormatLogEntry(logLevel, _categoryName, message, exception);

        File.AppendAllText(filePath, logEntry);
    }

    private string FormatLogEntry(LogLevel logLevel, string category, string message, Exception exception)
    {
        var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logLevelClass = logLevel.ToString().ToLower();

        var html = $@"
        <div class='log-entry {logLevelClass}'>
            <span class='time'>{time}</span>
            <span class='level'>{logLevel}</span>
            <span class='category'>{category}</span>
            <div class='message'>{message}</div>
            {(exception != null ? $"<div class='exception'>{exception}</div>" : "")}
        </div>";

        return html;
    }
    private void InitializeStyles(string filePath)
    {

        // Inicializar archivo con estilos si no existe
        if (!File.Exists(filePath))
        {
            var cssStyles = @"
        <style>
            .log-entry { margin: 5px 0; padding: 8px; border-left: 4px solid #ccc; }
            .time { color: #666; margin-right: 10px; }
            .level { font-weight: bold; margin-right: 10px; }
            .trace { border-color: #aaa; }
            .debug { border-color: #5bc0de; }
            .information { border-color: #5cb85c; }
            .warning { border-color: #f0ad4e; }
            .error { border-color: #d9534f; }
            .critical { border-color: #d9534f; background-color: #f2dede; }
            .exception { color: #d9534f; margin-top: 5px; }
        </style>
        ";
            File.WriteAllText(filePath, $"<html><head>{cssStyles}</head><body>");
        }
        else
        {
            // Append to the existing file
            File.AppendAllText(filePath, "<body>");
        }
    }
}