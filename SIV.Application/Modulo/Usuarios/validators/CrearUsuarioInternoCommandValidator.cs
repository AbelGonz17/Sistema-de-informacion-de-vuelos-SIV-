using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class CrearUsuarioInternoCommandValidator : AbstractValidator<CrearUsuarioInternoCommand>
    {
        public CrearUsuarioInternoCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CorreoElectronico).NotEmpty().EmailAddress();
            RuleFor(x => x.Contrasena).NotEmpty().MinimumLength(6);

            RuleFor(x => x.Rol)
                .Must(rol => rol == RolesConstantes.Operador || rol == RolesConstantes.Auditor)
                .WithMessage("El administrador solo puede dar de alta cuentas con roles de 'Operador' o 'Auditor'.");
        }
    }
}