namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class ResultadoCargaMasivaDto
    {
        public int TotalProcesados { get; set; }
        public int TotalExitosos { get; set; }
        public int TotalErrores { get; set; }
        public List<ErrorFilaDto> Errores { get; set; } = new List<ErrorFilaDto>();
    }

    public class ErrorFilaDto
    {
        public int Fila { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
