namespace BackEnd.DTOs.Admin
{
    public class AdminPedidoResumenDTO
    {
        public int PedId { get; set; }
        public string PedEstado { get; set; } = string.Empty;
        public decimal PedTotal { get; set; }
        public DateTime PedFechaCompra { get; set; }
        public DateOnly? PedFechaEntrega { get; set; }
        public string? PedMetTipoPago { get; set; }
        public string PedDirCalle { get; set; } = string.Empty;
        public string PedDirDistrito { get; set; } = string.Empty;
        public string PedDirProvincia { get; set; } = string.Empty;
        public string PedDirDepartamento { get; set; } = string.Empty;

        // Datos del cliente/usuario
        public string CliNombre { get; set; } = string.Empty;
        public string CliApellido { get; set; } = string.Empty;
        public string UsuEmail { get; set; } = string.Empty;
    }
}
