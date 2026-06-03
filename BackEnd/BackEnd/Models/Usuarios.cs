namespace BackEnd.Models
{
    public class Usuarios
    {
        public int UsuId { get; set; }
        public string UsuEmail { get; set; } = string.Empty; //no nulo
        public string UsuUsername { get; set; } = string.Empty; //no nulo
        public string UsuContrasenaHash { get; set; } = string.Empty; //no nulo
        public string UsuRol { get; set; } = "cliente"; //no nulo, default: "cliente"
        public DateTime? UsuFechaRegistro { get; set; } //puede ser nulo

        //Navigation Properties
        public Clientes? Cliente { get; set; } //1:1 con clientes (hija)
        //Aca si puede ser nulo porque si es admin no es cliente
        public Administradores? Administrador { get; set; } //1:1 con administradores (hija)
        
    }
}
