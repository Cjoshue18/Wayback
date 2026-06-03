namespace BackEnd.Models
{
    public class Pedidos
    {
        public int PedId { get; set; }
        public int CliId { get; set; }
        public int MetId { get; set; }
        public int DirId { get; set; }
        public string PedEstado { get; set; } = "pendiente";
        public decimal PedTotal { get; set; }
        public DateTime PedFechaCompra {  get; set; } //No puede ser nulo
        public DateOnly? PedFechaEntrega { get; set; } //puede ser nulo
        public string? PedPasarelaCargoId { get; set; }
        
        //Navigation Properties
        public Clientes Cliente { get; set; } = null!; //pertenece a un Cliente
        public MetodosPago? MetodoPago { get; set; } //tiene o no un metodo de pago
        public Direcciones? Direccion {  get; set; } //tiene o no direccion
        public ICollection<PedidoDetalles> Detalles { get; set; } = new List<PedidoDetalles>();
    }
}
