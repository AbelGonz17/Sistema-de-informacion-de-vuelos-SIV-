using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class OlvideContrasenaCommandHandler : IRequestHandler<OlvideContrasenaCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public OlvideContrasenaCommandHandler(IUsuarioRepository usuarioRepository, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(OlvideContrasenaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(request.CorreoElectronico);
            if (usuario == null)
            {
                // Para no revelar qué correos existen en el sistema, retornamos éxito siempre.
                return Result<bool>.Success(true);
            }

            // Generar token
            var token = Guid.NewGuid().ToString("N");
            usuario.GenerarTokenRecuperacion(token, 15); // Válido por 15 minutos

            await _usuarioRepository.ActualizarAsync(usuario);
            await _unitOfWork.CommitAsync();

            var enlaceRecuperacion = $"{request.UrlBaseFrontend.TrimEnd('/')}/restablecer-contrasena?token={token}&email={Uri.EscapeDataString(usuario.Correo)}";
            
            var mensaje = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #2b5797;'>Recuperación de Contraseña</h2>
                    <p>Hola {usuario.Nombre},</p>
                    <p>Has solicitado restablecer tu contraseña en el Sistema de Información de Vuelos (SIV). Haz clic en el siguiente botón para crear una nueva contraseña:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{enlaceRecuperacion}' style='background-color: #2b5797; color: white; padding: 12px 20px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Restablecer Contraseña</a>
                    </div>
                    <p>Este enlace expirará en 15 minutos.</p>
                    <p>Si el botón no funciona, puedes copiar y pegar el siguiente enlace en tu navegador:</p>
                    <p><a href='{enlaceRecuperacion}'>{enlaceRecuperacion}</a></p>
                    <p>Si no fuiste tú quien solicitó este cambio, por favor ignora este correo. Tu contraseña actual seguirá siendo válida.</p>
                    <br>
                    <p>Saludos,<br>El equipo de SIV</p>
                </div>
            ";

            await _emailService.SendEmailAsync(usuario.Correo, "Recuperación de Contraseña - SIV", mensaje);

            return Result<bool>.Success(true);
        }
    }
}
