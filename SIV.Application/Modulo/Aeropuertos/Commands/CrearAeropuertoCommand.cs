using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record CrearAeropuertoCommand(
        string Codigo, 
        string Nombre,
        string Pais
    ) : IRequest<Result<Guid>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<Guid> result && result.IsSuccess)
            {
                return $"Se registró el nuevo aeropuerto {Nombre} ({Codigo}) en {Pais} (ID: {result.Value}).";
            }
            return $"Intento de registrar el aeropuerto {Nombre} ({Codigo}) en {Pais} no fue completado.";
        }
    }
}