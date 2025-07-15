using AppCapasCitas.DTO.Enums;

namespace AppCapasCitas.DTO.Helpers;

public class PaisesHelper
{

    //usar CodigoPisEnum
    public static readonly Dictionary<int, string> Paises = new()
    {
        { (int)CodigoPaisEnum.MEX, "México" },
        { (int)CodigoPaisEnum.USA, "Estados Unidos" },
        { (int)CodigoPaisEnum.ESP, "España" },
        { (int)CodigoPaisEnum.COL, "Colombia" },
        { (int)CodigoPaisEnum.ARG, "Argentina" },
        { (int)CodigoPaisEnum.PER, "Perú" },
        { (int)CodigoPaisEnum.CHL, "Chile" }
    };
    public static string GetNombrePais(int codigo)
    {
        return Paises.TryGetValue(codigo, out var nombre) ? nombre : "Desconocido";
    }
}
