using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record ActualizarEstadoVueloCommand(Guid VueloId, EstadoVuelo NuevoEstado, string MotivoCambio) 
        : IRequest<Result<bool>>, IComandoOperativo;
}