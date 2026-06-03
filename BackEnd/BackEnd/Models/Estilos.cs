namespace BackEnd.Models
{
    public class Estilos
    {
        public int EstId { get; set; }
        public string EstNombre { get; set; } = string.Empty;

        //NavigationProperties
        public ICollection<Productos> Productos { get; set; } = new List<Productos>();
    }
}
