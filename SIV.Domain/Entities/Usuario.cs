using SIV.Domain.Interfaces;

namespace SIV.Domain.Entities
{
    public class Usuario : ISoftDeletable
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Rol { get; private set; }
        public string PassWordHash { get; private set; }
        public bool Activo { get; private set; } = true;
        private readonly List<Seguimiento> _seguimientos = new();
        private readonly List<Notificacion> _notificaciones = new();

        public IReadOnlyCollection<Seguimiento> Seguimientos => _seguimientos.AsReadOnly();
        public IReadOnlyCollection<Notificacion> Notificaciones => _notificaciones.AsReadOnly();

        private Usuario() { }

        public Usuario(
            Guid id, 
            string nombre, 
            string correo, 
            string rol, 
            string passWordHash)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Rol = rol;
            PassWordHash = passWordHash;
            Activo = true;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void IniciarSeguimiento(Vuelo vuelo)
        {
            var seguimientoActivo = _seguimientos.FirstOrDefault(s => s.VueloId == vuelo.Id && s.Activo);
            if (seguimientoActivo == null)
            {
                _seguimientos.Add(new Seguimiento
                {
                    UsuarioId = this.Id,
                    VueloId = vuelo.Id,
                    FechaInicio = DateTime.UtcNow,
                    Activo = true
                });
            }
        }

        public void DejarDeSeguir(Vuelo vuelo)
        {
            var seguimientoActivo = _seguimientos.FirstOrDefault(s => s.VueloId == vuelo.Id && s.Activo);
            if (seguimientoActivo != null)
            {
                seguimientoActivo.Activo = false;
                seguimientoActivo.FechaFin = DateTime.UtcNow;
            }
        }
    }
}