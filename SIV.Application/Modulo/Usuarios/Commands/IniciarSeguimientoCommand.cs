using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public class IniciarSeguimientoCommand : IRequest<Result<bool>>, IComandoSeguimiento
    {
        public Guid UsuarioId { get; set; }
        public Guid VueloId { get; set; } 
    }
}