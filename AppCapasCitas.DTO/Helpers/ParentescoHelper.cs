namespace AppCapasCitas.DTO.Helpers;

public static class ParentescoHelper
{
    public static readonly Dictionary<int, string> Parentescos = new()
    {
        { 1, "Padre" },
        { 2, "Madre" },
        { 3, "Hijo" },
        { 4, "Hija" },
        { 5, "Hermano" },
        { 6, "Hermana" },
        { 7, "Abuelo" },
        { 8, "Abuela" },
        { 9, "Tío" },
        { 10, "Tía" },
        { 11, "Primo" },
        { 12, "Prima" },
        { 13, "Otro" }
    };

    public static string GetNombreParentesco(int codigo)
    {
        return Parentescos.TryGetValue(codigo, out var nombre) ? nombre : "Desconocido";
    }
}