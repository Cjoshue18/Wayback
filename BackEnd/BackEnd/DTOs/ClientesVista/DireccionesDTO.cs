namespace BackEnd.DTOs.ClientesVista
{
    public class DireccionesDTO
    {

        public string DirCalle { get; set; } = string.Empty;
        public string DirDistrito { get; set; } = string.Empty;
        public string DirProvincia { get; set; } = string.Empty;
        public string DirDepartamento { get; set; } = string.Empty;
        public string? DirReferencia { get; set; } //puede ser nulo
        public bool DirPreferido { get; set; } = false;
    }
}
