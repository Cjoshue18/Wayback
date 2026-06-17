namespace BackEnd.DTOs.Public
{
    public class VariantesDetalleDTO
    {
        public int VarId {  get; set; }
        public string Talla { get; set; } = string.Empty;
        public string ColorNombre { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
