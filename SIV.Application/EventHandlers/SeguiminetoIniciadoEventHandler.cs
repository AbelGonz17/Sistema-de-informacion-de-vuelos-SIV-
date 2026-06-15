using MediatR;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class SeguimientoIniciadoEventHandler : INotificationHandler<SeguimientoIniciadoEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public SeguimientoIniciadoEventHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(SeguimientoIniciadoEvent notification, CancellationToken cancellationToken)
        {
            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.UsuarioActor,
                "IniciarSeguimiento",
                $"El usuario inició exitosamente el seguimiento personalizado del vuelo con ID: {notification.VueloId}."
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}