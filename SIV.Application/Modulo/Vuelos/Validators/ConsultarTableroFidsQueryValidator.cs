using FluentValidation;
using SIV.Application.Modulo.Vuelos.Queries;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class ConsultarTableroFidsQueryValidator : AbstractValidator<ConsultarTableroFidsQuery>
    {
        public ConsultarTableroFidsQueryValidator()
        {
            RuleFor(q => q.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("El número de página debe ser mayor o igual a 1.");

            RuleFor(q => q.PageSize)
                .GreaterThan(0).WithMessage("El tamaño de página debe ser mayor a 0.");
        }
    }
}