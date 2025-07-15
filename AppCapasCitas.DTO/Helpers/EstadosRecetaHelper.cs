

namespace AppCapasCitas.DTO.Helpers;

public class EstadosRecetaHelper
{
    public static readonly Dictionary<int, string> EstadosReceta = new()
    {
        { 1, "Pendiente" },
        { 2, "Aprobada" },
        { 3, "Anulada" },
        { 4, "Rechazada" }
       
    };

    public static string GetNombreEstado(int codigo)
    {
        return EstadosReceta.TryGetValue(codigo, out var nombre) ? nombre : "Desconocido";
    }

}
