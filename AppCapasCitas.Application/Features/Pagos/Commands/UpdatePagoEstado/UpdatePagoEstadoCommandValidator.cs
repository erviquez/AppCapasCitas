using FluentValidation;

namespace AppCapasCitas.Application.Features.Pagos.Commands.UpdatePagoEstado;

public class UpdatePagoEstadoCommandValidator : AbstractValidator<UpdatePagoEstadoCommand>
{
    public UpdatePagoEstadoCommandValidator()
    {
        RuleFor(x => x.PagoId)
            .NotEmpty().WithMessage("El ID del pago es requerido");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado es requerido")
            .MaximumLength(20).WithMessage("El estado no puede tener más de 20 caracteres")
            .Must(BeValidEstado).WithMessage("Estado no válido. Estados permitidos: Pendiente, Completado, Reembolsado, Fallido");

        RuleFor(x => x.Notas)
            .MaximumLength(500).WithMessage("Las notas no pueden tener más de 500 caracteres");
    }

    private bool BeValidEstado(string? estado)
    {
        if (string.IsNullOrEmpty(estado))
            return false;
            
        var estadosValidos = new[] { "Pendiente", "Completado", "Reembolsado", "Fallido" };
        return estadosValidos.Contains(estado);
    }
}