using FluentValidation;
using SIV.Application.Modulo.Aerolineas.Commands;

namespace SIV.Application.Modulo.Aerolineas.Validators
{
    public class CrearAerolineaCommandValidator : AbstractValidator<CrearAerolineaCommand>
    {
        public CrearAerolineaCommandValidator()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty().WithMessage("El código de la aerolínea es obligatorio.")
                .MaximumLength(5).WithMessage("El código no puede exceder los 5 caracteres.");

            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre de la aerolínea es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }
}
