using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ActualizarUsuarioInternoCommandHandler : IRequestHandler<ActualizarUsuarioInternoCommand, Result<string>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public ActualizarUsuarioInternoCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<Result<string>> Handle(ActualizarUsuarioInternoCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(request.Id);

            if (usuario == null)
                return Result<string>.Failure("Usuario no encontrado.", StatusCodes.Status404NotFound);  

            usuario.ActualizarPerfil(request.Nombre, request.Rol);

            await _usuarioRepository.ActualizarAsync(usuario);

            return Result<string>.Success(usuario.Id.ToString());
        }
    }
}