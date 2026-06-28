using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record MarcarNotificacionLeidaCommand(Guid NotificacionId) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se marcó como leída la notificación con ID {NotificacionId}.";
            }
            return $"Intento de marcar como leída la notificación con ID {NotificacionId} no fue completado.";
        }
    }
}