using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class DejarDeSeguirCommandValidator : AbstractValidator<DejarDeSeguirCommand>
    {
        public DejarDeSeguirCommandValidator()
        {
            RuleFor(c => c.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El ID del vuelo es obligatorio.");
        }
    }
}