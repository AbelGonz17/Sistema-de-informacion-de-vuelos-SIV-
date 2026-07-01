using FluentValidation;
using SIV.Application.Modulo.Vuelos.Queries;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class BuscarVueloEspecificoQueryValidator : AbstractValidator<BuscarVueloEspecificoQuery>
    {
        public BuscarVueloEspecificoQueryValidator()
        {
            RuleFor(q => q.NumeroVuelo)
                .NotEmpty().WithMessage("El número de vuelo es obligatorio para la búsqueda.");
        }
    }
}