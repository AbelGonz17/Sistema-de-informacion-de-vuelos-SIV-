namespace SIV.Domain.Entities
{
    public class Aeropuerto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
    }
}