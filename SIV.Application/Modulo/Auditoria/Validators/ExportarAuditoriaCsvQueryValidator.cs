using FluentValidation;
using SIV.Application.Modulo.Auditoria.Queries;

namespace SIV.Application.Modulo.Auditoria.Validators
{
    public class ExportarAuditoriaCsvQueryValidator : AbstractValidator<ExportarAuditoriaCsvQuery>
    {
        public ExportarAuditoriaCsvQueryValidator()
        {
            RuleFor(q => q.FechaInicio)
                .LessThanOrEqualTo(q => q.FechaFin)
                .When(q => q.FechaInicio.HasValue && q.FechaFin.HasValue)
                .WithMessage("La fecha de inicio debe ser menor o igual a la fecha de fin.");
        }
    }
}
