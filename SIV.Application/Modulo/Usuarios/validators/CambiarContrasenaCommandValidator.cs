using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class CambiarContrasenaCommandValidator : AbstractValidator<CambiarContrasenaCommand>
    {
        public CambiarContrasenaCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

            RuleFor(c => c.ContrasenaActual)
                .NotEmpty().WithMessage("La contraseña actual es obligatoria.");

            RuleFor(c => c.NuevaContrasena)
                .NotEmpty().WithMessage("La nueva contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La nueva contraseña debe tener al menos 8 caracteres.")
                .NotEqual(c => c.ContrasenaActual).WithMessage("La nueva contraseña debe ser diferente a la actual.");
        }
    }
}
