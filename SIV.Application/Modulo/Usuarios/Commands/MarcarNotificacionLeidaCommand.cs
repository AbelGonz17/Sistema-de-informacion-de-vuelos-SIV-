using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record MarcarNotificacionLeidaCommand(Guid NotificacionId) : IRequest<Result<bool>>, IComandoCatalogo;
}