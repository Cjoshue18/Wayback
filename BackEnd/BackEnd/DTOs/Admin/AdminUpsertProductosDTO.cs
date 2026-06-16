using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Admin
{
    public class AdminUpsertProductosDTO
    {
        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre del producto debe tener un máximo de 100 caracteres.")]
        public string ProNombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción del producto debe tener un máximo de 500 caracteres.")]
        public string? ProDescripcion { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(10, ErrorMessage = "El género del producto debe tener un máximo de 10 caracteres.")]
        [RegularExpression("^(Masculino|Femenino|Unisex)$",
            ErrorMessage = "El género debe ser masculino, femenino o unisex.")]
        public string ProGenero { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo Obligatorio.")]
        public int CatId { get; set; } 
        public int? EstId { get; set; }
        
        [Required(ErrorMessage = "Campo Obligatorio.")]
        [Range(0.10, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")] //Para que tenga un mínimo precio de 10 centimos
        public decimal ProPrecio { get; set; }
        public short? ProDescuento { get; set; }
        public DateTime? ProDescuentoInicio { get; set; }
        public DateTime? ProDescuentoFin {  get; set; }
    }
}
