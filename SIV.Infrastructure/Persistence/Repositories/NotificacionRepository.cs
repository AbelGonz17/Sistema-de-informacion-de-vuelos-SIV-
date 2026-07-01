using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities.Sistema;
using SIV.Domain.Interfaces;
using SIV.Infrastructure.Persistence;

namespace SIV.Infrastructure.Persistence.Repositories
{
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AgregarRangoAsync(IEnumerable<Notificacion> notificaciones)
        {
            await _context.Notificaciones.AddRangeAsync(notificaciones);
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .Where(n => n.UsuarioDestinatarioId == usuarioId)
                .OrderByDescending(n => n.FechaHoraGenearicion)
                .ToListAsync();
        }

        public async Task<Notificacion?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Notificaciones.FindAsync(id);
        }

        public Task ActualizarAsync(Notificacion notificacion)
        {
            _context.Notificaciones.Update(notificacion);
            return Task.CompletedTask;
        }
    }
}