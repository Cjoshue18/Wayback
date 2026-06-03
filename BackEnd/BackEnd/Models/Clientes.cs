namespace BackEnd.Models
{
    public class Clientes
    {
        public int CliId { get; set; } //PK
        public int UsuId { get; set; } //Fk
        public string CliDocument { get; set; } = string.Empty; //no nulo
        public string CliDocumentType { get; set; } = string.Empty;
        public string CliName { get; set; } = string.Empty;
        public string CliLastName { get; set; } = string.Empty;
        public string? CliPhone { get; set; } //puede ser nulo
        public string? CliPasarelaId { get; set; } //puede ser nulo
        public Usuarios? Usuario { get; set; } //1:1 con Usuarios (padre)
        //Propiedad de navegación para la relación con Usuarios
    }
}
