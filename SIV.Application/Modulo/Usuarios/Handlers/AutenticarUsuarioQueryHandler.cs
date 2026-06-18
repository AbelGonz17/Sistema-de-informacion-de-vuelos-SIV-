using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class AutenticarUsuarioQueryHandler : IRequestHandler<AutenticarUsuarioQuery, Result<string>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AutenticarUsuarioQueryHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<string>> Handle(AutenticarUsuarioQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(request.Correo);

            if (usuario == null)
                return Result<string>.Failure("Las credenciales ingresadas son incorrectas.",StatusCodes.Status400BadRequest);

            bool contraseñaValida = _passwordHasher.Verify(request.Contrasena, usuario.PassWordHash);

            if (!contraseñaValida)
                return Result<string>.Failure("Las credenciales ingresadas son incorrectas.",StatusCodes.Status400BadRequest);

            return Result<string>.Success(_tokenService.GenerarToken(usuario));
        }
    }
}