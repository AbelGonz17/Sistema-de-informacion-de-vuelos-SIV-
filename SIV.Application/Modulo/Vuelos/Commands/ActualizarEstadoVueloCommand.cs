using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class ActualizarEstadoVueloCommand : IRequest<bool>, IComandoOperativo
    {
        public Guid VueloId { get; set; }
        public EstadoVuelo NuevoEstado { get; set; }
        public string MotivoCambio { get; set; } = string.Empty;
    }
}