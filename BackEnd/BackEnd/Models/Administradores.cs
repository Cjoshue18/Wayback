namespace BackEnd.Models
{
    public class Administradores
    {
        public int AdId { get; set; }
        public int UsuId { get; set; } //FK
        public string AdNombre { get; set; } = string.Empty; //no nulo

        //Navigation Properties
        public Usuarios Usuario { get; set; } = null!;//1:1 con Usuarios (padre)
    }
}
