namespace BackEnd.Models
{
    public class Pedidos
    {
        public int PedId { get; set; }
        public int CliId { get; set; }

        public int? MetId { get; set; }
        //Snapshot de Método de Pago
        public string? PedMetTipoPago { get; set; }
        public string? PedMetUltimos4 { get; set; }

        //Mantenemos la referencia para analisis
        public int? DirId { get; set; }
        //Hacemos un snapshot o captura de los datos en ese momento al igual que los precios en detalle de pedidos
        public string PedDirCalle { get; set; } = string.Empty;
        public string PedDirDistrito { get; set; } = string.Empty;
        public string PedDirProvincia { get; set; } = string.Empty;
        public string PedDirDepartamento { get; set; } = string.Empty;
        public string? PedDirReferencia { get; set; }
        //Hasta aca es la snapshot del contenido de la tabla Direcciones

        public string PedEstado { get; set; } = "pendiente";
        public decimal PedTotal { get; set; }
        public string? PedPasarelaCargoId { get; set; }
        public DateTime PedFechaCompra {  get; set; } //No puede ser nulo
        public DateOnly? PedFechaEntrega { get; set; } //puede ser nulo
        
        //Navigation Properties
        public Clientes Cliente { get; set; } = null!; //pertenece a un Cliente
        public MetodosPago? MetodoPago { get; set; } //tiene o no un metodo de pago
        public Direcciones? Direccion {  get; set; } //tiene o no direccion
        public ICollection<PedidoDetalles> Detalles { get; set; } = new List<PedidoDetalles>();
    }
}
