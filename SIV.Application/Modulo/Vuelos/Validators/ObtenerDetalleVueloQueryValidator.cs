using FluentValidation;
using SIV.Application.Modulo.Vuelos.Queries;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class ObtenerDetalleVueloQueryValidator : AbstractValidator<ObtenerDetalleVueloQuery>
    {
        public ObtenerDetalleVueloQueryValidator()
        {
            RuleFor(q => q.VueloId)
                .NotEmpty().WithMessage("El ID del vuelo es obligatorio.");
        }
    }
}