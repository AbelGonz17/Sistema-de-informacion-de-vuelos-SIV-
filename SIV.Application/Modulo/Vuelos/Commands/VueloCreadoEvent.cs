using MediatR;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class VueloCreadoEvent : INotification
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Aerolinea { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
    }
}