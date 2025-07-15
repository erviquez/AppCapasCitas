using System;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Commands.UpdatePagoEstado;

public class UpdatePagoEstadoCommand : IRequest<Response<bool>>
{
    [Required]
    public Guid PagoId { get; set; }

    [Required]
    [StringLength(20)]
    public string? Estado { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}