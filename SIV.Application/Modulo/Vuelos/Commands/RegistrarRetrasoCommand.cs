using MediatR;
using SIV.Application.Common.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class RegistrarRetrasoCommand : IRequest<bool>, IComandoOperativo
    {
        public Guid VueloId { get; set; }
        public DateTime NuevaHoraSalida { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}