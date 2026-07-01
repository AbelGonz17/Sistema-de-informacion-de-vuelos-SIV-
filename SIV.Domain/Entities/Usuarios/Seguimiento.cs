using SIV.Domain.Entities.Vuelos;

namespace SIV.Domain.Entities.Usuarios
{
    public class Seguimiento
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid VueloId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Activo { get; set; }
        public Vuelo Vuelo { get; set; }
    }
}