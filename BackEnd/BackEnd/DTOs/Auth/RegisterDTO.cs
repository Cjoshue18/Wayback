using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required (ErrorMessage = "Campo Obligatorio.")] 
        [EmailAddress (ErrorMessage = "Escribir un correo electrónico válido.")]
        [MaxLength (100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MinLength (6, ErrorMessage = "El usuario debe tener mínimo 6 caracteres.")]
        [MaxLength (50, ErrorMessage = "El usuario debe tener un máximo de 50 caracteres.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres.")]
        public string Contrasena {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength (50)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(20)]
        public string TipoDocumento {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(12)]
        public string Documento {  get; set; } = string.Empty;
    }
}
