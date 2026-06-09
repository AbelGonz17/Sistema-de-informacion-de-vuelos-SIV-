using MediatR;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class AutenticarUsuarioQueryHandler : IRequestHandler<AutenticarUsuarioQuery, string>
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

        public async Task<string> Handle(AutenticarUsuarioQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(request.Correo);

            if (usuario == null)
                throw new InvalidOperationException("Las credenciales ingresadas son incorrectas.");

            bool contraseñaValida = _passwordHasher.Verify(request.Contrasena, usuario.PassWordHash);

            if (!contraseñaValida)
                throw new InvalidOperationException("Las credenciales ingresadas son incorrectas.");

            return _tokenService.GenerarToken(usuario);
        }
    }
}