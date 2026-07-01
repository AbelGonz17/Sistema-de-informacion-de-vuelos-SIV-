using FluentValidation;
using SIV.Application.Modulo.Aerolineas.Commands;

namespace SIV.Application.Modulo.Aerolineas.Validators
{
    public class EliminarAerolineaCommandValidator : AbstractValidator<EliminarAerolineaCommand>
    {
        public EliminarAerolineaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("El ID de la aerolínea es obligatorio para eliminarla.");
        }
    }
}
