using System;

namespace AppCapasCitas.Application.Features.Shared;
//parametros que se reciben para paginacion
public class PaginationBaseQuery
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    private const int MaxPagesize = 1;
    private int _pageSize = 3;
    public int PageSize { 
         get => _pageSize;
        set => _pageSize = (value > MaxPagesize)? MaxPagesize : value;
    } 
}