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

        public async Task<Usuario?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<Usuario?> ObtenerPorIdConVuelosAsync(Guid usuarioId)
        {
            return await _context.Usuarios
                .AsNoTracking() 
                .Include(u => u.Seguimientos.Where(s => s.Activo))
                    .ThenInclude(s => s.Vuelo)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);
        }

        public async Task<Usuario?> ObtenerParaModificacionAsync(Guid usuarioId)
        {
            return await _context.Usuarios
                .Include(u => u.Seguimientos)
                    .ThenInclude(s => s.Vuelo)
                        .ThenInclude(v => v.AerolineaRef)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);
        }

        public Task ActualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<string>> ObtenerSeguidoresDeVueloAsync(Guid vueloId)
        {
            return await _context.Usuarios
                 .AsNoTracking() 
                 .Where(u => _context.Seguimientos.Any(s => s.UsuarioId == u.Id && s.VueloId == vueloId && s.Activo))
                 .Select(u => u.Correo)
                 .ToListAsync();
        }
    }
}