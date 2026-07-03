namespace BackEnd.DTOs.Public
{
    public class ProductoTarjetaDTO
    {
        public int ProId { get; set; }
        public string ProNombre { get; set; } = string.Empty;
        public decimal ProPrecio { get; set; }
        public short? ProDescuento { get; set; }
        public DateTime? ProDescuentoInicio { get; set; }
        public DateTime? ProDescuentoFin { get; set; }
        public string Categoria {  get; set; }  = string.Empty;
        public string? Estilo { get; set; }
        public List<ColorTarjetaDTO> Colores { get; set; } = new List<ColorTarjetaDTO>();
    }
}
