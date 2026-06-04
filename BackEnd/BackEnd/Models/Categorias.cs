namespace BackEnd.Models
{
    public class Categorias
    {
        public int CatId { get; set; }
        public string CatNombre { get; set; } = string.Empty;

        //Navigation Properties
        public ICollection<Productos> Productos { get; set; } = new List<Productos>();
        //Una categoria tiene muchos productos
    }
}
