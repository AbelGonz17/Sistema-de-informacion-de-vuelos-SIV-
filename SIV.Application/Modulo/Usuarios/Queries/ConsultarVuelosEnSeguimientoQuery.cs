using MediatR;
using SIV.Application.Common.Mappings;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public class ConsultarVuelosEnSeguimientoQuery : IRequest<IEnumerable<VueloDto>>
    {
        public Guid UsuarioId { get; set; }
    }
}