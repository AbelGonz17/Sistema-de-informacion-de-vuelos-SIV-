using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record DesactivarUsuarioCommand(Guid UsuarioId) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se desactivó de forma permanente la cuenta del usuario con ID {UsuarioId}.";
            }
            return $"Intento de desactivar la cuenta del usuario con ID {UsuarioId} no fue completado.";
        }
    }
}
