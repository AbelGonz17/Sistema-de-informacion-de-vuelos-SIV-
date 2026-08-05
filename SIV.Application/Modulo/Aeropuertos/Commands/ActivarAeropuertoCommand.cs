using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record ActivarAeropuertoCommand(Guid Id) : IRequest<Result<bool>>;
}
