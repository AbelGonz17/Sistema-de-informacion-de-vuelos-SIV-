namespace SIV.Application.Common.Mappings
{
    public class VueloTableroDto
    {
        public Guid Id { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public Guid Aerolinea { get; set; }
        public Guid Origen { get; set; }
        public Guid Destino { get; set; }
        public DateTime HorarioPlanificado { get; set; }
        public DateTime? HorarioEstimado { get; set; }
        public string Puerta { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}