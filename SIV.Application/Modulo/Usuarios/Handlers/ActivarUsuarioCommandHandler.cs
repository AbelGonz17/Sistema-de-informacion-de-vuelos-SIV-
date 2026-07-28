using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ActivarUsuarioCommandHandler : IRequestHandler<ActivarUsuarioCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ActivarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<bool>> Handle(ActivarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(request.UsuarioId);

            if (usuario == null)
                return Result<bool>.Failure("El usuario especificado no existe.", StatusCodes.Status404NotFound);

            usuario.Activar();
            await _usuarioRepository.ActualizarAsync(usuario);

            return Result<bool>.Success(true);
        }
    }
}
