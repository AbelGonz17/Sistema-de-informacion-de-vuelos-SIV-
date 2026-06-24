using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class DesactivarUsuarioCommandHandler : IRequestHandler<DesactivarUsuarioCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public DesactivarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<bool>> Handle(DesactivarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(request.UsuarioId);

            if (usuario == null)
                return Result<bool>.Failure("El usuario especificado no existe.", StatusCodes.Status404NotFound);

            usuario.Desactivar();
            await _usuarioRepository.ActualizarAsync(usuario);

            return Result<bool>.Success(true);
        }
    }
}