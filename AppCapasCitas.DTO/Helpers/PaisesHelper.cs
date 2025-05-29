namespace AppCapasCitas.DTO.Helpers;

public static class PaisesHelper
{
    public static readonly Dictionary<int, string> Paises = new()
    {
        { 52, "México" },
        { 1, "Estados Unidos" },
        { 34, "España" },
        { 57, "Colombia" },
        
    };

    public static string GetNombrePais(int codigo)
    {
        return Paises.TryGetValue(codigo, out var nombre) ? nombre : "Desconocido";
    }
}
