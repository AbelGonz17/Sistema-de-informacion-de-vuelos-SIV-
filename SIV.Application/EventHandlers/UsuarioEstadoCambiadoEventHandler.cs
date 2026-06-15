using MediatR;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class UsuarioEstadoCambiadoEventHandler : INotificationHandler<UsuarioEstadoCambiadoEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public UsuarioEstadoCambiadoEventHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(UsuarioEstadoCambiadoEvent notification, CancellationToken cancellationToken)
        {
            string estadoTexto = notification.NuevoEstado ? "Activada" : "Desactivada";

            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.UsuarioActor,
                "GestionarUsuarioInterno", 
                $"El administrador cambió el estado de la cuenta ID {notification.UsuarioId} a: {estadoTexto}."
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}