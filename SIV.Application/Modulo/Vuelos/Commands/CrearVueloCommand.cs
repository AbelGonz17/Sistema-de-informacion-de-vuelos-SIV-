using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record CrearVueloCommand(
        string NumeroVuelo, 
        Guid Aerolinea, 
        Guid Origen, 
        Guid Destino, 
        DateTime HorarioPlanificadoSalida, 
        DateTime HorarioPlanificadoLlegada, 
        string Puerta
    ) : IRequest<Result<Guid>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<Guid> result && result.IsSuccess)
            {
                return $"Se programó exitosamente el nuevo vuelo {NumeroVuelo} (ID: {result.Value}).";
            }
            return $"Intento de registrar el vuelo {NumeroVuelo} no fue completado.";
        }
    }
}