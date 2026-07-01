using FluentValidation;
using SIV.Application.Modulo.Aerolineas.Queries;

namespace SIV.Application.Modulo.Aerolineas.Validators
{
    public class ObtenerAerolineaPorIdQueryValidator : AbstractValidator<ObtenerAerolineaPorIdQuery>
    {
        public ObtenerAerolineaPorIdQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty().WithMessage("El ID de la aerolínea es obligatorio para realizar la búsqueda.");
        }
    }
}
