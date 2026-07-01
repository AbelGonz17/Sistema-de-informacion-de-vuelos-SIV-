using FluentValidation;
using SIV.Application.Modulo.Vuelos.Commands;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class CancelarVueloCommandValidator : AbstractValidator<CancelarVueloCommand>
    {
        public CancelarVueloCommandValidator()
        {
            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El ID del vuelo es obligatorio.");

            RuleFor(c => c.Motivo)
                .NotEmpty().WithMessage("El motivo de la cancelación es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.");
        }
    }
}