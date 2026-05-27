namespace BackEnd.Models
{
    public class Clientes
    {
        public int Cli_ID { get; set; }
        public string Cli_Document { get; set; } = string.Empty;
        public string Cli_DocumentType { get; set; } = string.Empty;
        public string Cli_Name { get; set; } = string.Empty;
        public string Cli_LastName { get; set; } = string.Empty;
        public string Cli_Email { get; set; } = string.Empty;
        public string Cli_Phone { get; set; } = string.Empty;
        public string Cli_Username { get; set; } = string.Empty;
        public string Cli_PasswordHash { get; set; } = string.Empty;
        public string Cli_Stripe_ID { get; set; } = string.Empty;
        public DateTime Cli_RegisterDate { get; set; }
    }
}
