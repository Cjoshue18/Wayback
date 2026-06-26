using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.ClientesVista
{
    public class DetallePedidoCrearDTO
    {
        [Required]
        public int VarId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Cantidad { get; set; }
    }
}
