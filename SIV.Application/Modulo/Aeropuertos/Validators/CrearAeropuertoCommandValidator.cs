using FluentValidation;
using SIV.Application.Modulo.Aeropuertos.Commands;

namespace SIV.Application.Modulo.Aeropuertos.Validators
{
    public class CrearAeropuertoCommandValidator : AbstractValidator<CrearAeropuertoCommand>
    {
        public CrearAeropuertoCommandValidator()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty().WithMessage("El código del aeropuerto es obligatorio.")
                .MaximumLength(5).WithMessage("El código no puede exceder los 5 caracteres.");

            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre del aeropuerto es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(c => c.Pais)
                .NotEmpty().WithMessage("El país del aeropuerto es obligatorio.")
                .MaximumLength(100).WithMessage("El país no puede exceder los 100 caracteres.");
        }
    }
}
