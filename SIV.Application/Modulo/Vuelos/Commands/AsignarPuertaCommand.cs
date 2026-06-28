using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record AsignarPuertaCommand(Guid VueloId, string NuevaPuerta, string MotivoCambio) 
        : IRequest<Result<bool>>, IComandoOperativo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se asignó/cambió la puerta de embarque del vuelo con ID {VueloId} a '{NuevaPuerta}'. Motivo: {MotivoCambio}.";
            }
            return $"Intento de cambiar la puerta de embarque del vuelo con ID {VueloId} a '{NuevaPuerta}' no fue completado.";
        }
    }
}