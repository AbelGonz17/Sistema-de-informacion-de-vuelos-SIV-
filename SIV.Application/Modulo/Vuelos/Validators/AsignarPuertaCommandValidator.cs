using FluentValidation;
using SIV.Application.Modulo.Vuelos.Commands;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class AsignarPuertaCommandValidator : AbstractValidator<AsignarPuertaCommand>
    {
        public AsignarPuertaCommandValidator()
        {
            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El ID del vuelo es obligatorio.");

            RuleFor(c => c.NuevaPuerta)
                .NotEmpty().WithMessage("La nueva puerta es obligatoria.")
                .MaximumLength(10).WithMessage("La puerta no puede exceder los 10 caracteres.");

            RuleFor(c => c.MotivoCambio)
                .NotEmpty().WithMessage("El motivo del cambio es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.");
        }
    }
}