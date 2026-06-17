using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record CrearAerolineaCommand(
        string Codigo,
        string Nombre
    ) : IRequest<Result<Guid>>, IComandoCatalogo;
}