using System;

namespace AppCapasCitas.Transversal.Common;

public class ResponsePagination<T> : ResponseGeneric<T>
{
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; } // <-- Aquí defines el tamaño de página

    public bool HasPreviousPages => PageNumber > 1;
    public bool HasNextPages => PageNumber < TotalPages;
    

}
