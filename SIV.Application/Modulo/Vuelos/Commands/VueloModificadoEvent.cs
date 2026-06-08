using MediatR;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class VueloModificadoEvent : INotification
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string NuevoEstado { get; set; } = string.Empty;
    }
}