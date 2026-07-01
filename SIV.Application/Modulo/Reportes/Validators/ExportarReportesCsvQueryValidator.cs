using FluentValidation;
using SIV.Application.Modulo.Reportes.Queries;

namespace SIV.Application.Modulo.Reportes.Validators
{
    public class ExportarReportesCsvQueryValidator : AbstractValidator<ExportarReportesCsvQuery>
    {
        public ExportarReportesCsvQueryValidator()
        {
            RuleFor(q => q.TipoReporte)
                .NotEmpty().WithMessage("El tipo de reporte es obligatorio.");

            RuleFor(q => q.FechaInicio)
                .LessThanOrEqualTo(q => q.FechaFin)
                .When(q => q.FechaInicio.HasValue && q.FechaFin.HasValue)
                .WithMessage("La fecha de inicio debe ser menor o igual a la fecha de fin.");
        }
    }
}
