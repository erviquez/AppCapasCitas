using System;

namespace AppCapasCitas.Transversal.Common;
public interface IAppLogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogTrace(string message, params object[] args);
    void LogDebug(string message, params object[] args);
}
