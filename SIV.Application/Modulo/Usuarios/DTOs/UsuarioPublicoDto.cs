namespace SIV.Application.Modulo.Usuarios.DTOs
{
    public class UsuarioPublicoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
