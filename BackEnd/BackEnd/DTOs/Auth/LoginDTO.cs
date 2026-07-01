using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Auth
{
    public class LoginDTO
    {
        [Required (ErrorMessage = "Campo Obligatorio.")]
        [MinLength(3, ErrorMessage = "Debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "No puede tener más de 100 caracteres.")]
        public string UsuUsernameOrEmail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Campo Obligatorio.")]
        public string UsuContrasena { get; set; } = string.Empty;
    }
}
