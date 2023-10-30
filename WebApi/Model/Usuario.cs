namespace WebApi.Model
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Correo { get; set; }
        public string Username { get; set; }
        public string Rol { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }

    public class UsuarioDTO
    {
        public string Name { get; set; }
        public string Correo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }

    }


}
