using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class VueloCreadoEventHandler : INotificationHandler<VueloCreadoEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public VueloCreadoEventHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(VueloCreadoEvent notification, CancellationToken cancellationToken)
        {
            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.Usuario,
                "CrearVuelo",
                $"Se registró exitosamente el nuevo vuelo {notification.NumeroVuelo} de {notification.Aerolinea} con ruta {notification.Origen} -> {notification.Destino}."
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}