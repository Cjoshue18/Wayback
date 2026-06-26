using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.ClientesVista
{
    public class CrearPedidoYapeDTO
    {
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public int DirId { get; set; }

        [Required(ErrorMessage = "El número de Yape es obligatorio.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El número de Yape debe tener 9 dígitos.")]
        public string NumeroYape { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código de aprobación es obligatorio.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El código de aprobación debe tener 6 dígitos.")]
        public string CodigoAprobacion { get; set; } = string.Empty;

        [Required]
        public List<DetallePedidoCrearDTO> Items { get; set; } = new List<DetallePedidoCrearDTO>();
    }
}
