using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record RegistrarCuentaCommand(string Nombre, string Correo, string Contrasena) 
        : IRequest<Result<string>>, IComandoCatalogo;
}