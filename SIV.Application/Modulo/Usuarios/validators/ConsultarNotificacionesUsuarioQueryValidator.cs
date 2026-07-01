using FluentValidation;
using SIV.Application.Modulo.Usuarios.Queries;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class ConsultarNotificacionesUsuarioQueryValidator : AbstractValidator<ConsultarNotificacionesUsuarioQuery>
    {
        public ConsultarNotificacionesUsuarioQueryValidator()
        {
            RuleFor(q => q.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio para consultar notificaciones.");
        }
    }
}