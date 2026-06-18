using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public record ConsultarHistorialSeguimientosQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<HistorialSeguimientoDto>>>;
}