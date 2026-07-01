using FluentValidation;
using SIV.Application.Modulo.Aeropuertos.Commands;

namespace SIV.Application.Modulo.Aeropuertos.Validators
{
    public class EliminarAeropuertoCommandValidator : AbstractValidator<EliminarAeropuertoCommand>
    {
        public EliminarAeropuertoCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("El ID del aeropuerto es obligatorio para eliminarlo.");
        }
    }
}
