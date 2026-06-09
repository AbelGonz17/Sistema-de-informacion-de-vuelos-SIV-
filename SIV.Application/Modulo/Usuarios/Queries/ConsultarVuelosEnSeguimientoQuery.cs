using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public class ConsultarVuelosEnSeguimientoQuery : IRequest<Result<IEnumerable<VueloDto>>>
    {
        public Guid UsuarioId { get; set; }
    }
}