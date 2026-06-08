using MediatR;
using SIV.Application.Common.Mappings;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public class BuscarVueloEspecificoQuery : IRequest<VueloDto>
    {
        public string NumeroVuelo { get; set; } = string.Empty;
    }
}