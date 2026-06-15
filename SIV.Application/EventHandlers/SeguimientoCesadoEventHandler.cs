using MediatR;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class SeguimientoCesadoEventHandler : INotificationHandler<SeguimientoCesadoEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public SeguimientoCesadoEventHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(SeguimientoCesadoEvent notification, CancellationToken cancellationToken)
        {
            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.UsuarioActor,
                "CeseSeguimiento",
                $"El usuario removió exitosamente el vuelo con ID: {notification.VueloId} de su lista de seguimiento personalizado."
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}