namespace SIV.Application.Modulo.Aeropuertos.DTOs
{
    public class AeropuertoDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
    }
}