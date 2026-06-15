using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class CambiarEstadoUsuarioCommandValidator : AbstractValidator<CambiarEstadoUsuarioCommand>
    {
        public CambiarEstadoUsuarioCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El identificador del usuario es obligatorio.")
                .NotEqual(Guid.Empty).WithMessage("El identificador del usuario proporcionado no es válido.");

        }
    }
}