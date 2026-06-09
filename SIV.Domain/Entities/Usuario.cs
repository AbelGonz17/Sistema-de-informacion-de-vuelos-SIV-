namespace SIV.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Rol { get; private set; }
        public string PassWordHash { get; private set; }
        public ICollection<Vuelo> VuelosSeguidos { get; private set; } = new List<Vuelo>();

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
        }
    }
}