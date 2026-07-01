using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Admin
{
    public class AdminUpdatePedidoEstadoDTO
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [AllowedValues("pendiente", "aceptado", "rechazado", "cancelado", "entregado",
            ErrorMessage = "Estado no válido. Los valores permitidos son: pendiente, aceptado, rechazado, cancelado, entregado.")]
        public string PedEstado { get; set; } = string.Empty;
    }
}
