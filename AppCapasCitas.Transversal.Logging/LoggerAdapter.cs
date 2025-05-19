using AppCapasCitas.Transversal.Common;
using Microsoft.Extensions.Logging;

namespace AppCapasCitas.Transversal.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;
        private readonly HtmlFileLogger _htmlLogger;

        public LoggerAdapter(ILoggerFactory loggerFactory, string htmlLogFilePath = "logs.html")
        {
            _logger = loggerFactory.CreateLogger<T>();
            _htmlLogger = new HtmlFileLogger(htmlLogFilePath, typeof(T).Name);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger?.LogInformation(message, args);
            _htmlLogger.Log(LogLevel.Information, 0, message, null!, (s, e) => string.Format(s, args));
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger?.LogWarning(message, args);
            _htmlLogger.Log(LogLevel.Warning, 0, message, null!, (s, e) => string.Format(s, args));
        }

        public void LogError(string message, params object[] args)
        {
            _logger?.LogError(message, args);
            _htmlLogger.Log(LogLevel.Error, 0, message, null!, (s, e) => string.Format(s, args));
        }

        public void LogTrace(string message, params object[] args)
        {
            _logger?.LogTrace(message, args);
            _htmlLogger.Log(LogLevel.Trace, 0, message, null!, (s, e) => string.Format(s, args));
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger?.LogDebug(message, args);
            _htmlLogger.Log(LogLevel.Debug, 0, message, null!, (s, e) => string.Format(s, args));
        }
    }
}