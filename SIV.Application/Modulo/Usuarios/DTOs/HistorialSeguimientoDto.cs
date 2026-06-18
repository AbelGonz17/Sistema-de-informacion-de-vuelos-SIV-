namespace SIV.Application.Modulo.Usuarios.DTOs
{
    public class HistorialSeguimientoDto
    {
        public Guid SeguimientoId { get; set; }
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Aerolinea { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Activo { get; set; }
    }
}