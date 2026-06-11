using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public class BuscarVueloEspecificoQuery : IRequest<Result<VueloDto>>
    {
        public string NumeroVuelo { get; set; } = string.Empty;
    }
}