namespace BackEnd.DTOs.Auth
{
    public class LoginDTO
    {
        public string UsuUsername { get; set; } = string.Empty;
        public string UsuContrasenaHash { get; set; } = string.Empty;
    }
}
