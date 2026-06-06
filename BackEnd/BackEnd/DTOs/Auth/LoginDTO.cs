using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Auth
{
    public class LoginDTO
    {
        [Required (ErrorMessage = "Campo Obligatorio.")]
        [EmailAddress(ErrorMessage = "Escribir un correo electrónico válido.")]
        public string UsuUsernameOrEmail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Campo Obligatorio.")]
        public string UsuContrasena { get; set; } = string.Empty;
    }
}
