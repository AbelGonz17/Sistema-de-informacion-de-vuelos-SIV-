using FluentValidation;
using SIV.Application.Modulo.Vuelos.Commands;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class CrearVueloCommandValidator : AbstractValidator<CrearVueloCommand>
    {
        public CrearVueloCommandValidator()
        {
            RuleFor(c => c.NumeroVuelo)
                .NotEmpty().WithMessage("El número de vuelo es obligatorio.")
                .MaximumLength(20).WithMessage("El número de vuelo no puede exceder los 20 caracteres.");

            RuleFor(c => c.Aerolinea)
                .NotEmpty().WithMessage("La aerolínea es obligatoria.");


            RuleFor(c => c.Origen)
                .NotEmpty().WithMessage("El aeropuerto de origen es requerido.");

            RuleFor(c => c.Destino)
                .NotEmpty().WithMessage("El aeropuerto de destino es requerido.")
                .NotEqual(c => c.Origen).WithMessage("El destino no puede ser igual al origen.");

            RuleFor(c => c.HorarioPlanificadoSalida)
                .NotEmpty().WithMessage("La hora de salida es obligatoria.")
                .Must(fecha => fecha > DateTime.UtcNow).WithMessage("La fecha de salida planificada debe ser una fecha futura.");

            RuleFor(c => c.HorarioPlanificadoLlegada)
                .NotEmpty().WithMessage("La hora de llegada es obligatoria.")
                .Must((comando, llegada) => llegada > comando.HorarioPlanificadoSalida)
                .WithMessage("La fecha de llegada debe ser posterior a la fecha de salida.");

            RuleFor(c => c.Puerta)
                .NotEmpty().WithMessage("La puerta de embarque es obligatoria.")
                .MaximumLength(10).WithMessage("La puerta no puede exceder los 10 caracteres.");
        }
    }
}