using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class ActualizarUsuarioInternoCommandValidator : AbstractValidator<ActualizarUsuarioInternoCommand>
    {
        public ActualizarUsuarioInternoCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(c => c.Rol)
                .NotEmpty().WithMessage("El rol es obligatorio.");
        }
    }
}