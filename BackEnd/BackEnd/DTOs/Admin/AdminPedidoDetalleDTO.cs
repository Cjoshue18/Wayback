using System.Collections.Generic;

namespace BackEnd.DTOs.Admin
{
    public class AdminPedidoDetalleDTO
    {
        public int PedId { get; set; }
        public string PedEstado { get; set; } = string.Empty;
        public decimal PedTotal { get; set; }
        public DateTime PedFechaCompra { get; set; }
        public DateOnly? PedFechaEntrega { get; set; }
        public string? PedMetTipoPago { get; set; }
        public string? PedMetUltimos4 { get; set; }
        public string PedDirCalle { get; set; } = string.Empty;
        public string PedDirDistrito { get; set; } = string.Empty;
        public string PedDirProvincia { get; set; } = string.Empty;
        public string PedDirDepartamento { get; set; } = string.Empty;
        public string? PedDirReferencia { get; set; }

        // Datos del cliente/usuario
        public string CliNombre { get; set; } = string.Empty;
        public string CliApellido { get; set; } = string.Empty;
        public string UsuEmail { get; set; } = string.Empty;

        public List<AdminPedidoDetalleItemDTO> Detalles { get; set; } = new List<AdminPedidoDetalleItemDTO>();
    }

    public class AdminPedidoDetalleItemDTO
    {
        public int VarId { get; set; }
        public int DetPedCantidad { get; set; }
        public decimal DetPedPrecioUnitario { get; set; }
        public decimal DetPedSubTotal { get; set; }
        public string ProNombre { get; set; } = string.Empty;
        public string VarTalla { get; set; } = string.Empty;
        public string ColorNombre { get; set; } = string.Empty;
        public string? ImgURL { get; set; }
    }
}
