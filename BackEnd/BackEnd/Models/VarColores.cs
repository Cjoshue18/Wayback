namespace BackEnd.Models
{
    public class VarColores
    {
        public int ColorId { get; set; }
        public string ColorNombre { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;

        //Navigation Properties
        public ICollection<Variantes> Variantes { get; set; } = new List<Variantes>();
    }
}
