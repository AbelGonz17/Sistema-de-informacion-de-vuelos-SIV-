using FluentValidation;
using SIV.Application.Modulo.Usuarios.Commands;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class RefrescarTokenCommandValidator : AbstractValidator<RefrescarTokenCommand>
    {
        public RefrescarTokenCommandValidator()
        {
            RuleFor(c => c.AccessTokenViejo)
                .NotEmpty().WithMessage("El access token viejo es obligatorio.");

            RuleFor(c => c.RefreshTokenViejo)
                .NotEmpty().WithMessage("El refresh token viejo es obligatorio.");
        }
    }
}