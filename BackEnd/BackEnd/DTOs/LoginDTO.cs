namespace BackEnd.DTOs
{
    public class LoginDTO
    {
        public string UsuUsername { get; set; } = string.Empty;
        public string UsuPasswordHash { get; set; } = string.Empty;
    }
}
