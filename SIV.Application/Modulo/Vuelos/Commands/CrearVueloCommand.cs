using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class CrearVueloCommand : IRequest<Result<Guid>>
    {
        public string NumeroVuelo { get; set; } = string.Empty;
        public Guid Aerolinea { get; set; }
        public Guid Origen { get; set; }
        public Guid Destino { get; set; }
        public DateTime HorarioPlanificadoSalida { get; set; }
        public DateTime HorarioPlanificadoLlegada { get; set; } 
        public string Puerta { get; set; } = string.Empty;
    }
}