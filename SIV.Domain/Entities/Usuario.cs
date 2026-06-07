namespace SIV.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Rol { get; private set; }

        private Usuario() { }

        public Usuario(Guid id, string nombre, string correo, string rol)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Rol = rol;
        }
    }
}