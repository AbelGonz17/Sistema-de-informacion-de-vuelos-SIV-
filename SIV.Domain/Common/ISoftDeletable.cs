namespace SIV.Domain.Common
{
    public interface ISoftDeletable
    {
        bool Activo { get; }
        void Desactivar();
        void Activar();
    }
}
