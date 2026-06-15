namespace BackEnd.DTOs.Admin
{
    public class AdminGetProductosDTO
    {
        public int ProId { get; set; }
        public string ProNombre { get; set; } = string.Empty;
        public string ProGenero {  get; set; } = string.Empty;
        public string ProCategoria { get; set; } = string.Empty;
        public string? ProEstilo { get; set; }
        public decimal ProPrecio { get; set; }
        public short? ProDescuento { get; set; }
        public bool ProVigenteDescuento { get; set; }
        public DateTime? ProFechaCreacion {  get; set; }
        public int ProTotalStock { get; set; }

    }
}
