namespace BackEnd.Models
{
    public class Imagenes
    {
        public int ImgId { get; set; }
        public int ProId { get; set; }
        public string ImgURL { get; set; } = string.Empty;
        
        //Navigation Properties
        public Productos Producto { get; set; } = null!; //Una imagen pertenece a un producto
        public ICollection<Variantes> Variantes { get; set; } = new List<Variantes>();
        //Una imagen puede estar asignada a varias variantes
    }
}
