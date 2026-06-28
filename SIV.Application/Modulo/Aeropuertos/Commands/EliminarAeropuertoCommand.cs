using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record EliminarAeropuertoCommand(Guid Id) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se eliminó exitosamente el aeropuerto con ID {Id}.";
            }
            return $"Intento de eliminar el aeropuerto con ID {Id} no fue completado.";
        }
    }
}