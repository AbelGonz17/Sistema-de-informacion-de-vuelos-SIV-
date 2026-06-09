using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public class ConsultarTableroLlegadasQuery : IRequest<Result<IEnumerable<VueloDto>>>
    {
        public DateTime Fecha { get; set; }
        public bool EsLlegada { get; set; }
    }
}