using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Infrastructure.Persistence
{
    public class AeropuertoRepository : IAeropuertoRepository
    {
        private readonly ApplicationDbContext _context;

        public AeropuertoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aeropuerto>> ObtenerTodosAsync()
        {
            return await _context.Aeropuertos.AsNoTracking().ToListAsync();
        }

        public async Task AgregarAsync(Aeropuerto aeropuerto)
        {
            await _context.Aeropuertos.AddAsync(aeropuerto);
        }
    }
}
