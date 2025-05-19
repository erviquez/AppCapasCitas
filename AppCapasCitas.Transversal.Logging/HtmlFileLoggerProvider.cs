using Microsoft.Extensions.Logging;

namespace AppCapasCitas.Transversal.Logging;


public class HtmlFileLoggerProvider : ILoggerProvider
{
    private readonly string _basePath;
    
    public HtmlFileLoggerProvider(string basePath)
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
        Directory.CreateDirectory(Path.GetDirectoryName(_basePath)!);
    }
    
    public ILogger CreateLogger(string categoryName)
    {
        return new HtmlFileLogger(_basePath, categoryName);
    }
    
    public void Dispose() { }
}

