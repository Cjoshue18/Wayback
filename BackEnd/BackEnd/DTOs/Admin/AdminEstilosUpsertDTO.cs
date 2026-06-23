using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Admin
{
    public class AdminEstilosUpsertDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string EstNombre { get; set; } = string.Empty;
    }
}
