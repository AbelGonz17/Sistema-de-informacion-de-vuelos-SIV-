using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record CrearAeropuertoCommand(
        string Codigo, 
        string Nombre,
        string Pais
    ) : IRequest<Result<Guid>>, IComandoCatalogo;
}