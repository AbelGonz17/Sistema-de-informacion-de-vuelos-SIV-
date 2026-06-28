using MediatR;
using SIV.Application.Common.Events;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.EventHandlers
{
    public class AuditoriaEventHandler : INotificationHandler<AuditoriaEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuditoriaEventHandler(IAuditoriaRepository auditoriaRepository, IUnitOfWork unitOfWork)
        {
            _auditoriaRepository = auditoriaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AuditoriaEvent notification, CancellationToken cancellationToken)
        {
            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.Usuario,
                notification.Accion,
                notification.Detalles
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
