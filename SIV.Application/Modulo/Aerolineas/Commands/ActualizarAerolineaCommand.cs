using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record ActualizarAerolineaCommand(Guid Id, string Codigo, string Nombre) 
        : IRequest<Result<bool>>, IComandoCatalogo;
}