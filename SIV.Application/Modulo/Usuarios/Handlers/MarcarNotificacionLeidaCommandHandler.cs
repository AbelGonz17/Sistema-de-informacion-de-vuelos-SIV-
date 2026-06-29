using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class MarcarNotificacionLeidaCommandHandler : IRequestHandler<MarcarNotificacionLeidaCommand, Result<bool>>
    {
        private readonly INotificacionRepository _notificacionRepository;

        public MarcarNotificacionLeidaCommandHandler(INotificacionRepository notificacionRepository)
        {
            _notificacionRepository = notificacionRepository;
        }

        public async Task<Result<bool>> Handle(MarcarNotificacionLeidaCommand request, CancellationToken cancellationToken)
        {
            var notificacion = await _notificacionRepository.ObtenerPorIdAsync(request.NotificacionId);

            if (notificacion == null)
            {
                return Result<bool>.Failure("Notificación no encontrada.", StatusCodes.Status404NotFound);
            }

            if (notificacion.UsuarioDestinatarioId != request.UsuarioId)
            {
                return Result<bool>.Failure("No tienes permiso para marcar esta notificación como leída.", StatusCodes.Status403Forbidden);
            }

            if (!notificacion.FueLeida)
            {
                notificacion.FueLeida = true;
                await _notificacionRepository.ActualizarAsync(notificacion);
            }

            return Result<bool>.Success(true);
        }
    }
}