namespace AppCapasCitas.DTO.Helpers;

public static class GenerosHelper
{
    public static readonly Dictionary<int, string> Generos = new()
    {
        { 1, "Masculino" },
        { 2, "Femenino" },
        { 3, "Otro" },
        { 4, "No especificado" }
    };

    public static string GetNombreGenero(int codigo)
    {
        return Generos.TryGetValue(codigo, out var nombre) ? nombre : "Desconocido";
    }
}