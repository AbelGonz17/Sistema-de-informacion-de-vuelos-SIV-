using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record AsignarPuertaCommand(Guid VueloId, string NuevaPuerta, string MotivoCambio) 
        : IRequest<Result<bool>>, IComandoOperativo;
}