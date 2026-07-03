namespace BackEnd.DTOs.Public
{
    public class VariantesDetalleDTO
    {
        public int VarId {  get; set; }
        public string VarTalla { get; set; } = string.Empty;
        public string ColorNombre { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int VarStock { get; set; }
        public string? VarImgUrl { get; set; }
    }
}
