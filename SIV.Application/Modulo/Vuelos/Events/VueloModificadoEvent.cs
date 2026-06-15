using MediatR;

namespace SIV.Application.Modulo.Vuelos.Events
{
    public class VueloModificadoEvent : INotification
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string NuevoEstado { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string MotivoCambio { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
    }
}