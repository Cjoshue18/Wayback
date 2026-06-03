using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BackEnd.Models
{
    public class MetodosPago
    {
        public int MetId { get; set; }
        public int CliId { get; set; }
        public string? MetPasarelaCardId { get; set; }
        public string? MetPasarelaCardUltimos4 { get; set; }
        public string MetTipoPago { get; set; } = string.Empty;
        public bool MetPreferido { get; set; } = false;

        //Navigation Properties
        public Clientes Cliente { get; set; } = null!; //pertenece a un cliente
    }
}
