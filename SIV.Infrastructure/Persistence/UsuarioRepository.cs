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
            var usuario = await _context.Usuarios
                .Include(u => u.VuelosSeguidos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            var vuelo = await _context.Vuelos.FindAsync(vueloId);

            if (usuario != null && vuelo != null)
            {
                if (!usuario.VuelosSeguidos.Any(v => v.Id == vueloId))
                {
                    usuario.VuelosSeguidos.Add(vuelo);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task EliminarSeguimientoAsync(Guid usuarioId, Guid vueloId)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.VuelosSeguidos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario != null)
            {
                var vueloSeguido = usuario.VuelosSeguidos.FirstOrDefault(v => v.Id == vueloId);

                if (vueloSeguido != null)
                {
                    usuario.VuelosSeguidos.Remove(vueloSeguido);
                    await _context.SaveChangesAsync(); 
                }
            }
        }

        public async Task<IEnumerable<string>> ObtenerSeguidoresDeVueloAsync(Guid vueloId)
        {
            return await _context.Usuarios
                 .AsNoTracking() 
                 .Where(u => u.VuelosSeguidos.Any(v => v.Id == vueloId))
                 .Select(u => u.Correo)
                 .ToListAsync();
        }
    }
}