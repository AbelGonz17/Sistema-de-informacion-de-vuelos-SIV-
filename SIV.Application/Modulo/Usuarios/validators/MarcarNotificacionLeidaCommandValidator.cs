using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class MarcarNotificacionLeidaCommandValidator : AbstractValidator<MarcarNotificacionLeidaCommand>
    {
        public MarcarNotificacionLeidaCommandValidator()
        {
            RuleFor(c => c.NotificacionId)
                .NotEmpty().WithMessage("El ID de la notificación es obligatorio.");

            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");
        }
    }
}