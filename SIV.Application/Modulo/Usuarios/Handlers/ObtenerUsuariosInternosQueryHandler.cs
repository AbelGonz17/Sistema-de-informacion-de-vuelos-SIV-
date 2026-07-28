using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ObtenerUsuariosInternosQueryHandler : IRequestHandler<ObtenerUsuariosInternosQuery, Result<IEnumerable<UsuarioInternoDto>>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ObtenerUsuariosInternosQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<IEnumerable<UsuarioInternoDto>>> Handle(ObtenerUsuariosInternosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _usuarioRepository.ObtenerUsuariosInternosAsync();

            var dto = usuarios.Select(u => new UsuarioInternoDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Rol = u.Rol,
                Activo = u.Activo
            });

            return Result<IEnumerable<UsuarioInternoDto>>.Success(dto);
        }
    }
}
