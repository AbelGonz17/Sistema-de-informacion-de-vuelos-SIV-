using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class CerrarSesionCommandValidator : AbstractValidator<CerrarSesionCommand>
    {
        public CerrarSesionCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

            RuleFor(c => c.RefreshToken)
                .NotEmpty().WithMessage("El refresh token es obligatorio para cerrar sesión.");
        }
    }
}