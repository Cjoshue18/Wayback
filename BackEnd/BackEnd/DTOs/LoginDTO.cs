namespace BackEnd.DTOs
{
    public class LoginDTO
    {
        public string CliUsername { get; set; } = string.Empty;
        public string CliPasswordHash { get; set; } = string.Empty;
    }
}
