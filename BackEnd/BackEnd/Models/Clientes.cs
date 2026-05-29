namespace BackEnd.Models
{
    public class Clientes
    {
        public int CliId { get; set; }
        public string CliDocument { get; set; } = string.Empty; //no nulo
        public string CliDocumentType { get; set; } = string.Empty;
        public string CliName { get; set; } = string.Empty;
        public string CliLastName { get; set; } = string.Empty;
        public string CliEmail { get; set; } = string.Empty;
        public string? CliPhone { get; set; } //puede ser nulo
        public string CliUsername { get; set; } = string.Empty;
        public string CliPasswordHash { get; set; } = string.Empty;
        public string? CliStripeId { get; set; } //puede ser nulo
        public DateTime? CliRegisterDate { get; set; } //puede ser nulo
    }
}
