namespace BackEnd.Models
{
    public class PedidoDetalles
    {
        public int PedId { get; set; }
        public int VarId { get; set; }
        public int DetPedCantidad { get; set; } = 1;
        public decimal DetPedPrecioUnitario { get; set; }
        public decimal DetPedSubTotal { get; set; }

        //Navigation Properties
        public Pedidos Pedido { get; set; } = null!; //pertence si o si a un Pedido
        public Variantes Variante { get; set; } = null!; //no pertenece a una variante
        //pero si o si tiene que incluir una para que la orden exista
    }
}
