using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record CrearAerolineaCommand(
        string Codigo,
        string Nombre
    ) : IRequest<Result<Guid>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<Guid> result && result.IsSuccess)
            {
                return $"Se registró la nueva aerolínea {Nombre} ({Codigo}) (ID: {result.Value}).";
            }
            return $"Intento de registrar la aerolínea {Nombre} ({Codigo}) no fue completado.";
        }
    }
}