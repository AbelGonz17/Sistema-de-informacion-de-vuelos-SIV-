using MediatR;
using System;

namespace SIV.Application.Common.Events
{
    public class AuditoriaEvent : INotification
    {
        public string Usuario { get; }
        public string Accion { get; }
        public string Detalles { get; }
        public DateTime FechaRegistro { get; }

        public AuditoriaEvent(string usuario, string accion, string detalles)
        {
            Usuario = usuario;
            Accion = accion;
            Detalles = detalles;
            FechaRegistro = DateTime.UtcNow;
        }
    }
}
