namespace BackEnd.DTOs.Public
{
    public class ProductoTarjetaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int? Descuento { get; set; }
        public DateTime? FinDescuento { get; set; }
        public List<string>? ImagenesUrl { get; set; } = new List<string>();
        public string Categoria {  get; set; }  = string.Empty;
        public string? Estilo { get; set; }
        public List<string> Colores { get; set; } = new List<string>();
        public List<string> Tallas { get; set; } = new List<string>();
    }
}
