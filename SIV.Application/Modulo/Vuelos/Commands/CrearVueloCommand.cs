using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class CrearVueloCommand : IRequest<Result<Guid>>
    {
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Aerolinea { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public DateTime HorarioPlanificadoSalida { get; set; }
        public DateTime HorarioPlanificadoLlegada { get; set; } 
        public string Puerta { get; set; } = string.Empty;
    }
}