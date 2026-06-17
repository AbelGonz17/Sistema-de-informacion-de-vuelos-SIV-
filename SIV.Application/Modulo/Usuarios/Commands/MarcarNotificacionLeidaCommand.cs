using MediatR;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record MarcarNotificacionLeidaCommand(Guid NotificacionId) : IRequest<Result<bool>>;
}
