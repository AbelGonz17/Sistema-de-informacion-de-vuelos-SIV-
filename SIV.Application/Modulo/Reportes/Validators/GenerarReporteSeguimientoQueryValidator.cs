using FluentValidation;
using SIV.Application.Modulo.Reportes.Queries;

namespace SIV.Application.Modulo.Reportes.Validators
{
    public class GenerarReporteSeguimientoQueryValidator : AbstractValidator<GenerarReporteSeguimientoQuery>
    {
        public GenerarReporteSeguimientoQueryValidator()
        {
            RuleFor(q => q.Top)
                .GreaterThan(0).WithMessage("El valor de Top debe ser mayor a 0.");
        }
    }
}
