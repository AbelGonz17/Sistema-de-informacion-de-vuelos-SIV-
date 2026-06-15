using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class IniciarSeguimientoCommandValidator : AbstractValidator<IniciarSeguimientoCommand>
    {
        public IniciarSeguimientoCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El identificador del usuario es obligatorio.")
                .NotEqual(Guid.Empty).WithMessage("El identificador del usuario no es válido.");

            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El identificador del vuelo es obligatorio.")
                .NotEqual(Guid.Empty).WithMessage("El identificador del vuelo no es válido.");
        }
    }
}