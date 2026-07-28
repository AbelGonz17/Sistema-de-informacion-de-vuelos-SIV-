using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class ActivarUsuarioCommandValidator : AbstractValidator<ActivarUsuarioCommand>
    {
        public ActivarUsuarioCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");
        }
    }
}
