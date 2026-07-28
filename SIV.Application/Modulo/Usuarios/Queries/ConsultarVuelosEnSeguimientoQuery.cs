using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public record ConsultarVuelosEnSeguimientoQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<HistorialSeguimientoDto>>>;
}