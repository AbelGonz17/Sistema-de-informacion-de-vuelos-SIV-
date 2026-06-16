using MediatR;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class VueloCreadoEvent : INotification
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public Guid Aerolinea { get; set; }
        public Guid Origen { get; set; }
        public Guid Destino { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }
}