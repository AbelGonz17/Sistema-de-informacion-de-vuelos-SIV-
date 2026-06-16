namespace SIV.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Rol { get; private set; }
        public string PassWordHash { get; private set; }
        public IReadOnlyCollection<Seguimiento> Seguimientos { get; set; }
        public IReadOnlyCollection<Notificacion> Notificaciones { get; set; }

        private Usuario()
        {
            VuelosSeguidos = new HashSet<Vuelo>();
        }

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
            VuelosSeguidos = new HashSet<Vuelo>();
        }
    }
}