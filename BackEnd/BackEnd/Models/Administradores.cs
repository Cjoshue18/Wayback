namespace BackEnd.Models
{
    public class Administradores
    {
        public int AdId { get; set; }
        public int UsuId { get; set; } //FK
        public string AdName { get; set; } = string.Empty; //no nulo

        //Navigation Properties
        public Usuarios? Usuario { get; set; } //1:1 con Usuarios (padre)
    }
}
