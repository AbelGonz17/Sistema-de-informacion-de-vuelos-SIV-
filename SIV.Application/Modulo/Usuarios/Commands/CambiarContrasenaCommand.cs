using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record CambiarContrasenaCommand(Guid UsuarioId, string ContrasenaActual, string NuevaContrasena) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"El usuario con ID {UsuarioId} ha cambiado su contraseña.";
            }
            return $"Intento fallido de cambio de contraseña del usuario con ID {UsuarioId}.";
        }
    }
}
