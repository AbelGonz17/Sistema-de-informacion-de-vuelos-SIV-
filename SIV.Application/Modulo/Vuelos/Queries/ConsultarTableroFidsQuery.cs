using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Common.Models;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public class ConsultarTableroFidsQuery : IRequest<Result<PaginatedList<VueloTableroDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool? EsLlegada { get; set; }
        public string? Estado { get; set; }
        public Guid? AerolineaId { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
