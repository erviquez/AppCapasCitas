using System;

namespace AppCapasCitas.Application.Specifications;


public abstract class SpecificationParams
{
    public string? Sort { get; set; }
    public int PageIndex { get; set; } =1;
    private const int MaxPagesize = 50;
    private int _pageSize=10;
    public int PageSize{
        get => _pageSize;
        set => _pageSize = (value > MaxPagesize)? MaxPagesize : value;
    }
    public string? Search {get;set;}
}