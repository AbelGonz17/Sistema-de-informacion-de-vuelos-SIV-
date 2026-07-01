using FluentValidation;
using SIV.Application.Modulo.Aeropuertos.Queries;

namespace SIV.Application.Modulo.Aeropuertos.Validators
{
    public class ObtenerAeropuertoPorIdQueryValidator : AbstractValidator<ObtenerAeropuertoPorIdQuery>
    {
        public ObtenerAeropuertoPorIdQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty().WithMessage("El ID del aeropuerto es obligatorio para realizar la búsqueda.");
        }
    }
}
