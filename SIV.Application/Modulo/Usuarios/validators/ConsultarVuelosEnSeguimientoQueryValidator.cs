using FluentValidation;
using SIV.Application.Modulo.Usuarios.Queries;

namespace SIV.Application.Modulo.Usuarios.validators
{
    public class ConsultarVuelosEnSeguimientoQueryValidator : AbstractValidator<ConsultarVuelosEnSeguimientoQuery>
    {
        public ConsultarVuelosEnSeguimientoQueryValidator()
        {
            RuleFor(q => q.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio para consultar sus vuelos en seguimiento.");
        }
    }
}