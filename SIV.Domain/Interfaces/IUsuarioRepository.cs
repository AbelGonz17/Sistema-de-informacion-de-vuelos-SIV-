using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorIdAsync(Guid id);
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);
        Task<Usuario?> ObtenerPorIdConVuelosAsync(Guid usuarioId);
        Task<Usuario?> ObtenerParaModificacionAsync(Guid usuarioId);
        Task AgregarAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task<IEnumerable<string>> ObtenerSeguidoresDeVueloAsync(Guid vueloId);
        Task<IEnumerable<Guid>> ObtenerIdsSeguidoresDeVueloAsync(Guid vueloId);
    }
}