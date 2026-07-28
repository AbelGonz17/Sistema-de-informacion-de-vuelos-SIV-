using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using SIV.Application.Modulo.Usuarios.DTOs;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record RegistrarCuentaCommand(string Nombre, string Correo, string Contrasena) 
        : IRequest<Result<TokenResponseDto>>, IComandoCatalogo, IAuditableCommand
    {
        public string IpAddress { get; set; } = string.Empty;

        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<TokenResponseDto> result && result.IsSuccess)
            {
                return $"Se completó el registro de la cuenta pública para el usuario {Nombre} con correo {Correo}.";
            }
            return $"Intento de registrar la cuenta pública para {Nombre} ({Correo}) no fue completado.";
        }
    }
}