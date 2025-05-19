using System;

namespace AppCapasCitas.Application.Features.Shared;
//Parámetros de respuesta de paginación
public class PaginationVm<T> where T: class
{
    public int Count { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public IReadOnlyList<T>? Data {get;set;}
    public int PageCount { get; set; }

}