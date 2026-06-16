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
        Task AgregarAsync(Aerolinea aerolinea);
    }
}
