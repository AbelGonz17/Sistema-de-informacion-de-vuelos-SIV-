using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> ObtenerPorCorreoAsync(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task RegistrarSeguimientoAsync(Guid usuarioId, Guid vueloId)
        {
 
            await _context.SaveChangesAsync();
        }

        public async Task EliminarSeguimientoAsync(Guid usuarioId, Guid vueloId)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> ObtenerSeguidoresDeVueloAsync(Guid vueloId)
        {
            return new List<string>();
        }

    }
}