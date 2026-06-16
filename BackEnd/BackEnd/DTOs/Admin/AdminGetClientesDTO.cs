namespace BackEnd.DTOs.Admin
{
    public class AdminGetClientesDTO
    {
        public int CliId { get; set; }
        public string CliDocumento { get; set; } = string.Empty;
        public string CliTipoDocumento {  get; set; } = string.Empty;
        public string CliNombre { get; set; } = string.Empty;
        public string CliApellido { get; set; } = string.Empty;
        public string? CliTelefono { get; set; }
        public AdminUsuariosDTO? Usuario { get; set; }
    }
}
