using SIV.Domain.Common;
using SIV.Domain.Entities.Sistema;
using SIV.Domain.Entities.Vuelos;

namespace SIV.Domain.Entities.Usuarios
{
    public class Usuario : ISoftDeletable
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Rol { get; private set; }
        public string PassWordHash { get; private set; }
        public bool Activo { get; private set; } = true;
        public int IntentosFallidosLogin { get; private set; } = 0;
        public DateTime? BloqueadoHasta { get; private set; }

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

        public void Activar()
        {
            Activo = true;
        }



        public void CambiarContrasena(string nuevoHash)
        {
            if (string.IsNullOrWhiteSpace(nuevoHash)) throw new ArgumentException("El hash de la contraseña no puede estar vacío");
            PassWordHash = nuevoHash;
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

        public void RegistrarIntentoFallido(int limiteIntentos, int minutosBloqueo)
        {
            IntentosFallidosLogin++;
            if (IntentosFallidosLogin >= limiteIntentos)
            {
                BloqueadoHasta = DateTime.UtcNow.AddMinutes(minutosBloqueo);
            }
        }

        public void ResetearIntentos()
        {
            IntentosFallidosLogin = 0;
            BloqueadoHasta = null;
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

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public void AgregarRefreshToken(string token, int diasValidez, string ip)
        {
            _refreshTokens.Add(new RefreshToken
            {
                Token = token,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddDays(diasValidez),
                CreadoPorIp = ip
            });
        }

        public void RevocarRefreshToken(string token)
        {
            var rt = _refreshTokens.FirstOrDefault(t => t.Token == token);
            if (rt != null)
            {
                rt.Codificado = true;
            }
        }
        public void ActualizarPerfil(string nombre, string rol)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre no puede estar vacío");
            if (string.IsNullOrWhiteSpace(rol)) throw new ArgumentException("El rol no puede estar vacío");
            Nombre = nombre;
            Rol = rol;
        }


        public void RevocarTodosRefreshTokens()
        {
            foreach (var rt in _refreshTokens.Where(t => t.Activo))
            {
                rt.Codificado = true;
            }
        }
    }
}