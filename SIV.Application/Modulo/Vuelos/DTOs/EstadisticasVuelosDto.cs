using System.Collections.Generic;

namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class EstadisticasVuelosDto
    {
        public int TotalVuelos { get; set; }
        public double PorcentajeRetrasos { get; set; }
        public double PorcentajeCancelaciones { get; set; }
        public Dictionary<string, int> VuelosPorEstado { get; set; } = new Dictionary<string, int>();
    }
}
