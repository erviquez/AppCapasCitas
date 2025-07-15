using FluentValidation;

namespace AppCapasCitas.Application.Features.Pagos.Commands.CreatePago;

public class CreatePagoCommandValidator : AbstractValidator<CreatePagoCommand>
{
    public CreatePagoCommandValidator()
    {
        RuleFor(x => x.Monto)
            .NotEmpty().WithMessage("El monto es requerido")
            .GreaterThan(0).WithMessage("El monto debe ser mayor que 0")
            .LessThanOrEqualTo(99999.99m).WithMessage("El monto no puede ser mayor a $99,999.99");

        RuleFor(x => x.FechaPago)
            .NotEmpty().WithMessage("La fecha de pago es requerida");

        RuleFor(x => x.MetodoPago)
            .NotEmpty().WithMessage("El método de pago es requerido")
            .MaximumLength(20).WithMessage("El método de pago no puede tener más de 20 caracteres")
            .Must(BeValidMetodoPago).WithMessage("Método de pago no válido. Valores permitidos: Efectivo, Tarjeta, Transferencia");

        RuleFor(x => x.Comprobante)
            .MaximumLength(200).WithMessage("El comprobante no puede tener más de 200 caracteres");

        RuleFor(x => x.Notas)
            .MaximumLength(500).WithMessage("Las notas no pueden tener más de 500 caracteres");

        RuleFor(x => x.PacienteId)
            .NotEmpty().WithMessage("El ID del paciente es requerido");
    }

    private bool BeValidMetodoPago(string? metodoPago)
    {
        if (string.IsNullOrEmpty(metodoPago))
            return false;
            
        var metodosValidos = new[] { "Efectivo", "Tarjeta", "Transferencia" };
        return metodosValidos.Contains(metodoPago);
    }
}