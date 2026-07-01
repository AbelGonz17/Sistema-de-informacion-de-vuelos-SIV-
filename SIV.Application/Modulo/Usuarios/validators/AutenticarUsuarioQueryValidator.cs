using FluentValidation;
using SIV.Application.Modulo.Usuarios.Queries;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class AutenticarUsuarioQueryValidator : AbstractValidator<AutenticarUsuarioQuery>
    {
        public AutenticarUsuarioQueryValidator()
        {
            RuleFor(q => q.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.");

            RuleFor(q => q.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");
        }
    }
}