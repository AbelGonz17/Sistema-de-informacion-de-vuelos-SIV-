using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record ActivarUsuarioCommand(Guid UsuarioId) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se activó la cuenta del usuario con ID {UsuarioId}.";
            }
            return $"Intento de activar la cuenta del usuario con ID {UsuarioId} no fue completado.";
        }
    }
}
