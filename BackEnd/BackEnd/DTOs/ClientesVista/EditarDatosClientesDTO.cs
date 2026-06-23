using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.ClientesVista
{
    public class EditarDatosClientesDTO
    {
        [Required]
        [MaxLength(50)]
        public string CliNombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CliApellido { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "El usuario debe tener mínimo 6 caracteres.")]
        [MaxLength(50, ErrorMessage = "El usuario debe tener un máximo de 50 caracteres.")]
        public string UsuUsername {  get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Escribir un correo electrónico válido.")]
        [MaxLength(100)]
        public string UsuEmail {  get; set; } = string.Empty;

        [MaxLength(15)]
        public string? CliTelefono {  get; set; }
    }
}
