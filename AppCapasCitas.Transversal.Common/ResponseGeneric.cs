using System;
using FluentValidation.Results;

namespace AppCapasCitas.Transversal.Common;

public  class ResponseGeneric<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; } = false;
    public string? Message { get; set; }
    //Devuelve la lista de validaciones fallidas
    public IEnumerable<ValidationFailure>? Errors { get; set; }
}
