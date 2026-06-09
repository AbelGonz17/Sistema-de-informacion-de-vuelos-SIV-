using MediatR;
using SIV.Application.Common.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public class DejarDeSeguirCommand : IRequest<bool>, IComandoOperativo
    {
        public Guid UsuarioId { get; set; }
        public Guid VueloId { get; set; }
    }
}