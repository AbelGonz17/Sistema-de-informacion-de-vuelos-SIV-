using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;


namespace SIV.Application.Modulo.Usuarios.Handlers.Commands
{
    public class RegistrarCuentaCommandHandler : IRequestHandler<RegistrarCuentaCommand, Result<string>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMediator _mediator;

        public RegistrarCuentaCommandHandler(
            IUsuarioRepository usuarioRepository, 
            ITokenService tokenService, 
            IPasswordHasher passwordHasher,
            IMediator mediator)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _mediator = mediator;
        }

        public async Task<Result<string>> Handle(RegistrarCuentaCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente =
                await _usuarioRepository.ObtenerPorCorreoAsync(request.Correo);

            if (usuarioExistente != null)
                return Result<string>.Failure("El correo electrónico ya se encuentra registrado en el sistema.",StatusCodes.Status409Conflict);

            string passwordHash = _passwordHasher.Hash(request.Contrasena);

            var nuevoUsuario = new Domain.Entities.Usuario(
                Guid.NewGuid(),
                request.Nombre,
                request.Correo,
                RolesConstantes.Visitante,
                passwordHash);

            await _usuarioRepository.AgregarAsync(nuevoUsuario);

            await _mediator.Publish(new CuentaRegistradaEvent
            {
                UsuarioId = nuevoUsuario.Id,
                Correo = nuevoUsuario.Correo
            }, cancellationToken);

            return Result<string>.Success(_tokenService.GenerarToken(nuevoUsuario));
        }
    }
}