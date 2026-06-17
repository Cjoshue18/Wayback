namespace BackEnd.DTOs.Cliente
{
    public class EditarDatosClientesDTO
    {
        public string CliNombre { get; set; } = string.Empty;
        public string CliApellido { get; set; } = string.Empty;
        public string UsuUsername {  get; set; } = string.Empty;
        public string UsuEmail {  get; set; } = string.Empty;
        public string? CliTelefono {  get; set; }
    }
}
