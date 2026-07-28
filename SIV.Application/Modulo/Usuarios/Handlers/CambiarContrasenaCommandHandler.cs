using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class CambiarContrasenaCommandHandler : IRequestHandler<CambiarContrasenaCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CambiarContrasenaCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<bool>> Handle(CambiarContrasenaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(request.UsuarioId);

            if (usuario == null)
                return Result<bool>.Failure("El usuario especificado no existe.", StatusCodes.Status404NotFound);

            bool contrasenaValida = _passwordHasher.Verify(request.ContrasenaActual, usuario.PassWordHash);
            if (!contrasenaValida)
                return Result<bool>.Failure("La contraseña actual proporcionada es incorrecta.", StatusCodes.Status400BadRequest);

            string nuevoHash = _passwordHasher.Hash(request.NuevaContrasena);
            usuario.CambiarContrasena(nuevoHash);

            await _usuarioRepository.ActualizarAsync(usuario);

            return Result<bool>.Success(true);
        }
    }
}
