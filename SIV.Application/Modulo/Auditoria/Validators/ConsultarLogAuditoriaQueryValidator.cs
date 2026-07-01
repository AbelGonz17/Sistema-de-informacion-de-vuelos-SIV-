using FluentValidation;
using SIV.Application.Modulo.Auditoria.Queries;

namespace SIV.Application.Modulo.Auditoria.Validators
{
    public class ConsultarLogAuditoriaQueryValidator : AbstractValidator<ConsultarLogAuditoriaQuery>
    {
        public ConsultarLogAuditoriaQueryValidator()
        {
            RuleFor(q => q.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("El número de página debe ser mayor o igual a 1.");

            RuleFor(q => q.PageSize)
                .GreaterThan(0).WithMessage("El tamaño de página debe ser mayor a 0.");

            RuleFor(q => q.FechaInicio)
                .LessThanOrEqualTo(q => q.FechaFin)
                .When(q => q.FechaInicio.HasValue && q.FechaFin.HasValue)
                .WithMessage("La fecha de inicio debe ser menor o igual a la fecha de fin.");
        }
    }
}
