using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record ActivarAerolineaCommand(Guid Id) : IRequest<Result<bool>>;
}
