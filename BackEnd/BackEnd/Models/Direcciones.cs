namespace BackEnd.Models
{
    public class Direcciones
    {
        public int DirId { get; set; }
        public int CliId { get; set; }
        public string DirCalle { get; set; } = string.Empty;
        public string DirDistrito { get; set; } = string.Empty;
        public string DirProvincia {  get; set; } = string.Empty;
        public string DirDepartamento {  get; set; } = string.Empty;
        public string? DirReferencia {  get; set; } //puede ser nulo
        public bool DirPreferido { get; set; } = false;

        //Navigation Properties
        public Clientes Cliente { get; set; } = null!; //pertenece a un cliente
        public ICollection<Pedidos>? Pedidos { get; set; } = new List<Pedidos>(); //Una direccion puede o no estar en varios pedidos
    }
}
