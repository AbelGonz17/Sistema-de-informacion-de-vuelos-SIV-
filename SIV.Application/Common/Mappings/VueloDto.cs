namespace SIV.Application.Common.Mappings
{
    public class VueloDto
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public Guid Aerolinea { get; set; } 
        public Guid Origen { get; set; } 
        public Guid Destino { get; set; } 
        public DateTime HorarioPlanificadoSalida { get; set; }
        public DateTime? HorarioEstimadoSalida { get; set; }
        public string Puerta { get; set; } = string.Empty;
        public string EstadoActual { get; set; } = string.Empty;
    }
}