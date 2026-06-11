using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class VueloModificadoAuditoriaHandler : INotificationHandler<VueloModificadoEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public VueloModificadoAuditoriaHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(VueloModificadoEvent notification, CancellationToken cancellationToken)
        {
            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.Usuario,
                notification.Accion,
                notification.MotivoCambio
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}