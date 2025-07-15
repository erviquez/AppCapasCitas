using System;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Commands.CreatePago;

public class CreatePagoCommand : IRequest<Response<Guid>>
{
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Monto { get; set; }

    [Required]
    public DateTime FechaPago { get; set; } = DateTime.Now;

    [Required]
    [StringLength(20)]
    public string? MetodoPago { get; set; }

    [StringLength(200)]
    public string? Comprobante { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }

    [Required]
    public Guid PacienteId { get; set; }

    public Guid? CitaId { get; set; }
    public Guid? UsuarioCreacionId { get; set; }
}