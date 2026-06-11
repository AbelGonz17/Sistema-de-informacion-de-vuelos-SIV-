using FluentValidation;
using SIV.Application.Modulo.Vuelos.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class RegistrarRetrasoCommandValidator : AbstractValidator<RegistrarRetrasoCommand>
    {
        public RegistrarRetrasoCommandValidator()
        {
            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El identificador del vuelo es obligatorio.")
                .NotEqual(Guid.Empty).WithMessage("El identificador del vuelo proporcionado no es válido.");
    
            RuleFor(c => c.NuevaHoraSalida)
                .NotEmpty().WithMessage("La nueva hora de salida estimada es obligatoria.")
                .Must(HoraDebeSerFutura).WithMessage("La nueva hora de salida estimada no puede ser una fecha u hora pasada.");

            RuleFor(c => c.Motivo)
                .NotEmpty().WithMessage("Debe especificar obligatoriamente el motivo o justificación técnica del retraso.")
                .MaximumLength(500).WithMessage("El motivo del retraso no puede superar los 500 caracteres.");
        }

        private bool HoraDebeSerFutura(DateTime fechaProporcionada)
        {
            return fechaProporcionada > DateTime.UtcNow;
        }
    }
}
