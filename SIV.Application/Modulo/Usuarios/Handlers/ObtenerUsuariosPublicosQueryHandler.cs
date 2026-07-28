using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ObtenerUsuariosPublicosQueryHandler : IRequestHandler<ObtenerUsuariosPublicosQuery, Result<IEnumerable<UsuarioPublicoDto>>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ObtenerUsuariosPublicosQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<IEnumerable<UsuarioPublicoDto>>> Handle(ObtenerUsuariosPublicosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _usuarioRepository.ObtenerUsuariosPublicosAsync();

            var dto = usuarios.Select(u => new UsuarioPublicoDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Activo = u.Activo
            });

            return Result<IEnumerable<UsuarioPublicoDto>>.Success(dto);
        }
    }
}
