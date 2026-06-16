using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .Where(n => n.UsuarioDestinatarioId == usuarioId)
                .OrderByDescending(n => n.FechaHoraGenearicion)
                .ToListAsync();
        }

        public async Task AgregarAsync(Notificacion notificacion)
        {
            await _context.Notificaciones.AddAsync(notificacion);
        }

        public async Task AgregarRangoAsync(IEnumerable<Notificacion> notificaciones)
        {
            await _context.Notificaciones.AddRangeAsync(notificaciones);
        }

        public async Task MarcarComoLeidasAsync(IEnumerable<Guid> notificacionIds)
        {
            var notificaciones = await _context.Notificaciones
                .Where(n => notificacionIds.Contains(n.Id))
                .ToListAsync();

            foreach (var notificacion in notificaciones)
            {
                notificacion.FueLeida = true;
            }
        }
    }
}
