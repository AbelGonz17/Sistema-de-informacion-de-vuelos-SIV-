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
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> ObtenerPorIdConVuelosAsync(Guid usuarioId)
        {
            return await _context.Usuarios
                .AsNoTracking() 
                .Include(u => u.Seguimientos.Where(s => s.Activo))
                    .ThenInclude(s => s.Vuelo)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);
        }

        public async Task RegistrarSeguimientoAsync(Guid usuarioId, Guid vueloId)
        {
            var existe = await _context.Seguimientos
                .AnyAsync(s => s.UsuarioId == usuarioId && s.VueloId == vueloId && s.Activo);

            if (!existe)
            {
                var nuevoSeguimiento = new Seguimiento
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    VueloId = vueloId,
                    FechaInicio = DateTime.UtcNow,
                    Activo = true
                };

                await _context.Seguimientos.AddAsync(nuevoSeguimiento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarSeguimientoAsync(Guid usuarioId, Guid vueloId)
        {
            var seguimiento = await _context.Seguimientos
                .FirstOrDefaultAsync(s => s.UsuarioId == usuarioId && s.VueloId == vueloId && s.Activo);

            if (seguimiento != null)
            {
                seguimiento.FechaFin = DateTime.UtcNow;
                seguimiento.Activo = false;
                await _context.SaveChangesAsync(); 
            }
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