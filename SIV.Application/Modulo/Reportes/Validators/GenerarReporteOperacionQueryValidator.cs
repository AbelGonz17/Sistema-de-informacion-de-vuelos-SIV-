using FluentValidation;
using SIV.Application.Modulo.Reportes.Queries;

namespace SIV.Application.Modulo.Reportes.Validators
{
    public class GenerarReporteOperacionQueryValidator : AbstractValidator<GenerarReporteOperacionQuery>
    {
        public GenerarReporteOperacionQueryValidator()
        {
            RuleFor(q => q.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.")
                .LessThanOrEqualTo(q => q.FechaFin).WithMessage("La fecha de inicio debe ser menor o igual a la fecha de fin.");

            RuleFor(q => q.FechaFin)
                .NotEmpty().WithMessage("La fecha de fin es obligatoria.");
        }
    }
}
