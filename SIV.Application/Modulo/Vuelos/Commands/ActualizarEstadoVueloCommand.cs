using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using SIV.Domain.Entities.Vuelos;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record ActualizarEstadoVueloCommand(Guid VueloId, EstadoVuelo NuevoEstado, string MotivoCambio) 
        : IRequest<Result<bool>>, IComandoOperativo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se cambió exitosamente el estado del vuelo con ID {VueloId} a '{NuevoEstado}'. Motivo: {MotivoCambio}.";
            }
            return $"Intento de cambiar el estado del vuelo con ID {VueloId} a '{NuevoEstado}' no fue completado.";
        }
    }
}