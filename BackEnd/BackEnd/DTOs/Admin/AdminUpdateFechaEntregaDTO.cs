using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Admin
{
    public class AdminUpdateFechaEntregaDTO
    {
        // Nullable: enviar null para borrar la fecha de entrega
        public DateOnly? PedFechaEntrega { get; set; }
    }
}
