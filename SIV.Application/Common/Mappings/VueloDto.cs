namespace SIV.Application.Common.Mappings
{
    public class VueloDto
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Aerolinea { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public DateTime HorarioPlanificadoSalida { get; set; }
        public DateTime? HorarioEstimadoSalida { get; set; }
        public string Puerta { get; set; } = string.Empty;
        public string EstadoActual { get; set; } = string.Empty;
    }
}