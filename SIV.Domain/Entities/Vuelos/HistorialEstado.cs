namespace SIV.Domain.Entities.Vuelos
{
    public class HistorialEstado
    { 
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public EstadoVuelo EstadoAnterior { get; set; }
        public EstadoVuelo EstadoNuevo { get; set; }
        public DateTime FechaHora { get; set; }
        public Guid UsuarioResponsable { get; set; }
    }
}