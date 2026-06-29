using SIV.Domain.Interfaces;

namespace SIV.Domain.Entities
{
    public class Aeropuerto : ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public bool Activo { get; private set; } = true;

        public void Desactivar()
        {
            Activo = false;
        }
    }
}