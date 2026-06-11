using FluentValidation;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Validators
{
    public class ActualizarEstadoVueloCommandValidator : AbstractValidator<ActualizarEstadoVueloCommand>
    {
        public ActualizarEstadoVueloCommandValidator()
        {

            RuleFor(c => c.VueloId)
                .NotEmpty().WithMessage("El identificador del vuelo es obligatorio.")
                .NotEqual(Guid.Empty).WithMessage("El identificador del vuelo proporcionado no es válido.");

            RuleFor(c => c.NuevoEstado)
                .IsInEnum().WithMessage("El estado de vuelo especificado no es válido dentro del sistema.");

            RuleFor(c => c.MotivoCambio)
                .NotEmpty()
                .When(c => c.NuevoEstado == EstadoVuelo.Retrasado || c.NuevoEstado == EstadoVuelo.Cancelado)
                .WithMessage("Debe especificar obligatoriamente un motivo o justificación cuando el estado sea Retrasado o Cancelado.")
                .MaximumLength(500).WithMessage("El motivo del cambio no puede superar los 500 caracteres.");
        }
    }
}