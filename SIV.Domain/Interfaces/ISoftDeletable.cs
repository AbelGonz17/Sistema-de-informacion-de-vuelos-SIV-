namespace SIV.Domain.Interfaces
{
    public interface ISoftDeletable
    {
        bool Activo { get; }
        void Desactivar();
    }
}
