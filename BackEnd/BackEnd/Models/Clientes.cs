namespace BackEnd.Models
{
    public class Clientes
    {
        public int CliId { get; set; } //PK
        public int UsuId { get; set; } //Fk
        public string CliDocumento { get; set; } = string.Empty; //no nulo
        public string CliTipoDocumento { get; set; } = string.Empty;
        public string CliNombre { get; set; } = string.Empty;
        public string CliApellido { get; set; } = string.Empty;
        public string? CliTelefono { get; set; } //puede ser nulo
        public string? CliPasarelaId { get; set; } //puede ser nulo
        
        //Navigations Properties
        public Usuarios Usuario { get; set; } = null!; //1:1 con Usuarios (padre)
        //Propiedad de navegación para la relación con Usuarios
        //" = null" al inicio será null, separá un espacio, pero no debería ser null
        //usamos "!" para decirle al compilador que SI tendrá información, solo que no
        //instantaneamente al comienzo. Esto solo va para las tablas hijas porque si o si deben tener padre.

        //Clientes tambien es padre de otras tablas hijas, con relación 1:N, asi que necesitamos una List
        public ICollection<Direcciones> Direcciones { get; set; } = new List<Direcciones>();
        public ICollection<MetodosPago> MetodosPago { get; set; } = new List<MetodosPago>();
        public ICollection<Pedidos> Pedidos { get; set; } = new List<Pedidos>();
    }
}
