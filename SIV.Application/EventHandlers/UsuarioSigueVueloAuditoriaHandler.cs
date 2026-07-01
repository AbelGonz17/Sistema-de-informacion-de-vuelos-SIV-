using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Entities.Sistema;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class UsuarioSigueVueloAuditoriaHandler : INotificationHandler<UsuarioSigueVueloEvent>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public UsuarioSigueVueloAuditoriaHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(UsuarioSigueVueloEvent notification, CancellationToken cancellationToken)
        {
            var accionDesc = notification.Accion == "IniciarSeguimiento" 
                ? "Inició seguimiento" 
                : "Detuvo seguimiento";

            var log = new LogAuditoria(
                Guid.NewGuid(),
                notification.CorreoUsuario,
                notification.Accion,
                $"{accionDesc} del vuelo {notification.NumeroVuelo}."
            );

            await _auditoriaRepository.RegistrarLogAsync(log);
        }
    }
}