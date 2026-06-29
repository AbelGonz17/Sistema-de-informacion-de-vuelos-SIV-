using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public record ConsultarVuelosEnSeguimientoQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<VueloDto>>>;
}