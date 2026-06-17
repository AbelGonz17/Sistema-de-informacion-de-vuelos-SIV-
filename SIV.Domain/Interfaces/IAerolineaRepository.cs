using SIV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Domain.Interfaces
{
    public interface IAerolineaRepository
    {
        Task<IEnumerable<Aerolinea>> ObtenerTodasAsync();
        Task<Aerolinea?> ObtenerPorCodigoAsync(string codigo);
        Task<Aerolinea?> ObtenerPorIdAsync(Guid id);
        Task AgregarAsync(Aerolinea aerolinea);
        Task ActualizarAsync(Aerolinea aerolinea);
        Task EliminarAsync(Aerolinea aerolinea);
    }
}
