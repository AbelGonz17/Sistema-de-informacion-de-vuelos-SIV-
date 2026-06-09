using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class RegistrarCuentaCommandValidator : AbstractValidator<RegistrarCuentaCommand>
    {
        public RegistrarCuentaCommandValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(c => c.Correo)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
                .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres.");

            RuleFor(c => c.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}