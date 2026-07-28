using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities.Usuarios;
using SIV.Domain.Interfaces;
using SIV.Infrastructure.Persistence;

namespace SIV.Infrastructure.Persistence.Repositories
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
            return await _context.Usuarios.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);
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
                .IgnoreQueryFilters()
                .Include(u => u.Seguimientos)
                    .ThenInclude(s => s.Vuelo)
                        .ThenInclude(v => v.AerolineaRef)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);
        }

        public async Task<Usuario?> ObtenerPorCorreoConRefreshTokensAsync(string correo)
        {
            return await _context.Usuarios
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public Task ActualizarAsync(Usuario usuario)
        {
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

        public async Task<IEnumerable<Guid>> ObtenerIdsSeguidoresDeVueloAsync(Guid vueloId)
        {
            return await _context.Usuarios
                 .AsNoTracking()
                 .Where(u => _context.Seguimientos.Any(s => s.UsuarioId == u.Id && s.VueloId == vueloId && s.Activo))
                 .Select(u => u.Id)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosInternosAsync()
        {
            var rolesInternos = new[] { "Administrador", "Operador", "Auditor" };
            return await _context.Usuarios
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(u => rolesInternos.Contains(u.Rol))
                .OrderBy(u => u.Rol)
                .ThenBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosPublicosAsync()
        {
            return await _context.Usuarios
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(u => u.Rol == "Visitante")
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }
    }
}