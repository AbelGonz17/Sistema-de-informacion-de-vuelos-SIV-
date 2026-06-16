using SIV.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Domain.Interfaces
{
    public interface IAeropuertoRepository
    {
        Task<IEnumerable<Aeropuerto>> ObtenerTodosAsync();
        Task AgregarAsync(Aeropuerto aeropuerto);
    }
}
