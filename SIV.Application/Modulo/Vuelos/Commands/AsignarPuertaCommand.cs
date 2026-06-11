using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class AsignarPuertaCommand : IRequest<Result<bool>>, IComandoOperativo
    {
        public Guid VueloId { get; set; }
        public string NuevaPuerta { get; set; } = string.Empty;
        public string MotivoCambio { get; set; } = string.Empty;
    }
}