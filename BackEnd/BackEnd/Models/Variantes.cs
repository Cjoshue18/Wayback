namespace BackEnd.Models
{
    public class Variantes
    {
        public int VarId { get; set; }
        public int ProId { get; set; }
        public int ColorId { get; set; }
        public string VarTalla { get; set; } = "S";
        public int VarStock { get; set; } = 0;
        public decimal VarPrecio { get; set; }

        //Navigation Properties
        public Productos Producto { get; set; } = null!; //Pertenece a un producto
        public ICollection<VarColores> VarColores { get; set; } = new List<VarColores>();
        //Una variante puede tener muchos colores variantes
        public ICollection<PedidoDetalles> Detalles { get; set; } = new List<PedidoDetalles>();
        //Una variante puede estar en multiples pedidos
    }
}
