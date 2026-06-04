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
        public VarColores VarColor { get; set; } = null!; //Una variante tiene un color asignado
        //Una variante "P1" tiene tallas en S M y L, entonces son 3 variantes,
        //pero cada variante tiene SOLO un color asignado a esta.
        //Por lo tanto, la relacion es Variantes N:1 VarColores.    
        //Una variante SOLO tiene 1 color, pero un color puede pertenecer a varias variantes
        //(P1-S, P1-M, P1-L pueden ser del mismo color)
        public ICollection<PedidoDetalles> Detalles { get; set; } = new List<PedidoDetalles>();
        //Una variante puede estar en multiples pedidos
    }
}
