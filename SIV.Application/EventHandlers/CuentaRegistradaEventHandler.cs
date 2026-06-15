using MediatR;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class CuentaRegistradaEventHandler : INotificationHandler<CuentaRegistradaEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public CuentaRegistradaEventHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(CuentaRegistradaEvent notification, CancellationToken cancellationToken)
        {
            var log = new LogAuditoria(
                Guid.NewGuid(),
                "Anónimo", 
                "RegistrarCuenta",
                $"Se registró exitosamente una nueva cuenta de usuario con el correo: {notification.Correo}."
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}