using MediatR;
using SIV.Application.Common.Mappings;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public class ConsultarTableroLlegadasQuery : IRequest<IEnumerable<VueloDto>>
    {
        public DateTime Fecha { get; set; }
        public bool EsLlegada { get; set; }
    }
}