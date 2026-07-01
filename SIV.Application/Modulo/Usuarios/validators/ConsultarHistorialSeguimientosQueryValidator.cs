using FluentValidation;
using SIV.Application.Modulo.Usuarios.Queries;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class ConsultarHistorialSeguimientosQueryValidator : AbstractValidator<ConsultarHistorialSeguimientosQuery>
    {
        public ConsultarHistorialSeguimientosQueryValidator()
        {
            RuleFor(q => q.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio para consultar su historial de seguimientos.");
        }
    }
}