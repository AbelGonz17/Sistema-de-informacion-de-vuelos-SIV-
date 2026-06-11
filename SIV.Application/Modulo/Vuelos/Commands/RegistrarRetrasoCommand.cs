using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class RegistrarRetrasoCommand : IRequest<Result<bool>>, IComandoOperativo
    {
        public Guid VueloId { get; set; }
        public DateTime NuevaHoraSalida { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}