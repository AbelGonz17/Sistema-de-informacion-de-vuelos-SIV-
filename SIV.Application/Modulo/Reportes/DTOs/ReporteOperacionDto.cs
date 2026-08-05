namespace SIV.Application.Modulo.Reportes.DTOs
{
    public class ReporteOperacionDto
    {
        public int TotalVuelos { get; set; }
        public int Completados { get; set; }
        public int Cancelados { get; set; }
        public int Retrasados { get; set; }
        public int Otros { get; set; }

        public List<VueloOperacionDetalleDto> Vuelos { get; set; } = new();
        public List<VuelosPorDiaDto> VuelosPorDia { get; set; } = new();
    }

    public class VuelosPorDiaDto
    {
        public string Fecha { get; set; } = string.Empty;
        public int Total { get; set; }
    }

    public class VueloOperacionDetalleDto
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Aerolinea { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public DateTime HorarioPlanificadoSalida { get; set; }
        public string EstadoActual { get; set; } = string.Empty;
    }
}
