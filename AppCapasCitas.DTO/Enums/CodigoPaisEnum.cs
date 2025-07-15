
using System.ComponentModel.DataAnnotations;

namespace AppCapasCitas.DTO.Enums;

public enum CodigoPaisEnum
{    
        [Display(Name = "México")] MEX = 52,
        [Display(Name = "Estados Unidos")] USA = 1,
        [Display(Name = "España")] ESP = 34,
        [Display(Name = "Colombia")] COL = 57,
        [Display(Name = "Argentina")] ARG = 54,
        [Display(Name = "Perú")] PER = 51,
        [Display(Name = "Chile")] CHL = 56
}
