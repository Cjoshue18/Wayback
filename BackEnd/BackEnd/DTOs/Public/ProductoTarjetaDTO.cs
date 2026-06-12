namespace BackEnd.DTOs.Public
{
    public class ProductoTarjetaDTO
    {
        public int Id { get; set; }
        public string ProNombre { get; set; } = string.Empty;
        public decimal ProPrecio { get; set; }
        public short? ProDescuento { get; set; }
        public DateTime? ProDescuentoInicio { get; set; }
        public DateTime? ProDescuentoFin { get; set; }
        public List<string>? ImagenesUrl { get; set; } = new List<string>();
        public string Categoria {  get; set; }  = string.Empty;
        public string? Estilo { get; set; }
        public List<string> Colores { get; set; } = new List<string>();
        public List<string> Tallas { get; set; } = new List<string>();
    }
}
