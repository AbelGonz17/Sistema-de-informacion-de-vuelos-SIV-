using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record EliminarAeropuertoCommand(Guid Id) 
        : IRequest<Result<bool>>, IComandoCatalogo;
}