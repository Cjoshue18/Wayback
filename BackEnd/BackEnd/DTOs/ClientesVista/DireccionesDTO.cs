namespace BackEnd.DTOs.ClientesVista
{
    public class DireccionesDTO
    {
        public int DirId { get; set; } //para reusar para los pedidos
        public string DirCalle { get; set; } = string.Empty;
        public string DirDistrito { get; set; } = string.Empty;
        public string DirProvincia { get; set; } = string.Empty;
        public string DirDepartamento { get; set; } = string.Empty;
        public string? DirReferencia { get; set; } //puede ser nulo
        public bool DirPreferido { get; set; } = false;
    }
}
