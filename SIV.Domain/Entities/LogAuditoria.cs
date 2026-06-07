namespace SIV.Domain.Entities
{
    public class LogAuditoria
    {
        public Guid Id { get; private set; }
        public string Usuario { get; private set; }
        public string Accion { get; private set; }
        public string Detalles { get; private set; }
        public DateTime FechaRegistro { get; private set; }

        private LogAuditoria() { }

        public LogAuditoria(Guid id, string usuario, string accion, string detalles)
        {
            Id = id;
            Usuario = usuario;
            Accion = accion;
            Detalles = detalles;
            FechaRegistro = DateTime.UtcNow; 
        }
    }
}