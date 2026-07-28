using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public class ObtenerUsuariosInternosQuery : IRequest<Result<IEnumerable<UsuarioInternoDto>>>
    {
    }
}
