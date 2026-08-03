namespace SIV.Application.Modulo.Aerolineas.DTOs
{
    public class AerolineaDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}