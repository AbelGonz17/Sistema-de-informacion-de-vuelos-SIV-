namespace SIV.Application.Modulo.Reportes.DTOs
{
    public class VueloEstadoReporteDto
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class VueloMasSeguidoReporteDto
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public int CantidadSeguidores { get; set; }
    }
}