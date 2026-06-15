namespace SIV.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Rol { get; private set; }
        public string PassWordHash { get; private set; }
        public bool Activo { get; private set; } 
        public int IntentosFallidos { get; private set; } 
        public DateTime? BloqueoHasta { get; private set; } 
        public DateTime FechaRegistro { get; private set; }
        public ICollection<Vuelo> VuelosSeguidos { get; private set; }

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
            Correo = correo.ToLower().Trim();
            Rol = rol;
            PassWordHash = passWordHash;
            Activo = true; 
            IntentosFallidos = 0;
            FechaRegistro = DateTime.UtcNow;
            VuelosSeguidos = new HashSet<Vuelo>();
        }

        public bool EstaBloqueado => BloqueoHasta.HasValue && BloqueoHasta.Value > DateTime.UtcNow;

        public void RegistrarLoginExitoso()
        {
            IntentosFallidos = 0;
            BloqueoHasta = null;
        }

        public void RegistrarIntentoFallido(int maxIntentos = 5, int minutosBloqueo = 15)
        {
            IntentosFallidos++;
            if (IntentosFallidos >= maxIntentos)
            {
                BloqueoHasta = DateTime.UtcNow.AddMinutes(minutosBloqueo);
            }
        }

        public void CambiarEstadoActivo(bool nuevoEstado)
        {
            Activo = nuevoEstado; 
        }
    }
}