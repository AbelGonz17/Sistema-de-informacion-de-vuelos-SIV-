using FluentValidation;
using SIV.Application.Modulo.Vuelos.Queries;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class ObtenerHistorialVueloQueryValidator : AbstractValidator<ObtenerHistorialVueloQuery>
    {
        public ObtenerHistorialVueloQueryValidator()
        {
            RuleFor(q => q.VueloId)
                .NotEmpty().WithMessage("El ID del vuelo es obligatorio para obtener su historial.");
        }
    }
}