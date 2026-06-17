namespace BackEnd.DTOs.Public
{
    public class ProductoDescripcionDTO
    {
        public int ProId { get; set; }
        public string ProNombre { get; set; } = string.Empty;
        public string ProGenero { get; set; } = string.Empty;
        public string? ProDescripcion {  get; set; }
        public decimal ProPrecio { get; set; }
        public short? ProDescuento { get; set; }
        public DateTime? ProDescuentoInicio { get; set; }
        public DateTime? ProDescuentoFin { get; set; }
        public List<string>? ImagenesUrl { get; set; } = new List<string>();
        public string Categoria { get; set; } = string.Empty;
        public string? Estilo { get; set; }
        public List<VariantesDetalleDTO> Variantes { get; set; } = new List<VariantesDetalleDTO>();
    }
}
