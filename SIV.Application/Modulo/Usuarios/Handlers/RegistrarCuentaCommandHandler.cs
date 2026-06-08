using MediatR;
using SIV.Application.Modulo.Usuario.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;


namespace SIV.Application.Modulo.Usuario.Handlers
{
    public class RegistrarCuentaCommandHandler : IRequestHandler<RegistrarCuentaCommand,string>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public RegistrarCuentaCommandHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(RegistrarCuentaCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente =
                await _usuarioRepository.ObtenerPorCorreoAsync(request.Correo);

            if (usuarioExistente != null)
                throw new InvalidOperationException("El correo electrónico ya se encuentra registrado en el sistema.");

            string passwordHash = _passwordHasher.Hash(request.Contrasena);

            var nuevoUsuario = new Domain.Entities.Usuario(
                Guid.NewGuid(),
                request.Nombre,
                request.Correo,
                RolesConstantes.Administrador,
                passwordHash);

            await _usuarioRepository.AgregarAsync(nuevoUsuario);

            return _tokenService.GenerarToken(nuevoUsuario);
        }
    }
}