namespace BackEnd.DTOs.Cliente
{
    public class EditarDatosClientesDTO
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreUsuario {  get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string? Telefono {  get; set; }
    }
}
