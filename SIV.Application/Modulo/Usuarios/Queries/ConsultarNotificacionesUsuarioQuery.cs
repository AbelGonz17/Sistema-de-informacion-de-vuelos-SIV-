using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;
using System;
using System.Collections.Generic;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public record ConsultarNotificacionesUsuarioQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<NotificacionDto>>>;
}
