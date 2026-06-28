using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record EliminarAerolineaCommand(Guid Id) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se eliminó la aerolínea con ID {Id}.";
            }
            return $"Intento de eliminar la aerolínea con ID {Id} no fue completado.";
        }
    }
}