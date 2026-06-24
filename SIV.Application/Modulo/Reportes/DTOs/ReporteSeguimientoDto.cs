namespace SIV.Application.Modulo.Reportes.DTOs
{
    public class ReporteSeguimientoDto
    {
        public int TotalUsuariosConSeguimientosActivos { get; set; }
        public List<VueloMasSeguidoReporteDto> TopVuelosMasSeguidos { get; set; } = new();
    }

    public class VueloMasSeguidoReporteDto
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public int CantidadSeguidores { get; set; }
    }
}
