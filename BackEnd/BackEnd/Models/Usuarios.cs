namespace BackEnd.Models
{
    public class Usuarios
    {
        public int UsuId { get; set; }
        public string UsuEmail { get; set; } = string.Empty; //no nulo
        public string UsuUsername { get; set; } = string.Empty; //no nulo
        public string UsuPasswordHash { get; set; } = string.Empty; //no nulo
        public string UsuRole { get; set; } = "cliente"; //no nulo, default: "cliente"
        public DateTime? CliRegisterDate { get; set; } //puede ser nulo

        //Navigation Properties
        public Clientes? Cliente { get; set; } //1:1 con clientes (hija)
        public Administradores? Administrador { get; set; } //1:1 con administradores (hija)
    }
}
