using SIV.Domain.Common;

namespace SIV.Domain.Entities.Catalogo
{
    public class Aerolinea : ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; private set; } = true;

        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
        }
    }
}