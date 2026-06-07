using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerPorIdAsync(Guid id);
        Task<Usuario> ObtenerPorCorreoAsync(string correo);
        Task AgregarAsync(Usuario usuario);
        Task RegistrarSeguimientoAsync(Guid usuarioId, Guid vueloId);
        Task EliminarSeguimientoAsync(Guid usuarioId, Guid vueloId);
        Task<IEnumerable<string>> ObtenerSeguidoresDeVueloAsync(Guid vueloId);
    }
}