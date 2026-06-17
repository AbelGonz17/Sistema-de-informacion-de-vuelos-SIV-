using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record EliminarAerolineaCommand(Guid Id) 
        : IRequest<Result<bool>>, IComandoCatalogo;
}