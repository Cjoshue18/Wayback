namespace BackEnd.DTOs.Admin
{
    public class AdminUsuariosDTO
    {
        public int UsuId { get; set; }
        public string UsuEmail { get; set; } = string.Empty; 
        public string UsuUsername { get; set; } = string.Empty; 
        public DateTime? UsuFechaRegistro { get; set; } 
    }
}
