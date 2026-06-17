namespace SIV.Application.Common.Mappings
{
    public class VueloTableroDto
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Aerolinea { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public DateTime HorarioPlanificado { get; set; }
        public DateTime? HorarioEstimado { get; set; }
        public string Puerta { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}