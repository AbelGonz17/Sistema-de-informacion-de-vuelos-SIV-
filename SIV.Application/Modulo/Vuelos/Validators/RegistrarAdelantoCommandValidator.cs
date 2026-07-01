using FluentValidation;
using SIV.Application.Modulo.Vuelos.Commands;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class RegistrarAdelantoCommandValidator : AbstractValidator<RegistrarAdelantoCommand>
    {
        public RegistrarAdelantoCommandValidator()
        {
            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El ID del vuelo es obligatorio.");

            RuleFor(c => c.NuevaHoraSalida)
                .NotEmpty().WithMessage("La nueva hora de salida es obligatoria.");

            RuleFor(c => c.Motivo)
                .NotEmpty().WithMessage("El motivo del adelanto es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.");
        }
    }
}