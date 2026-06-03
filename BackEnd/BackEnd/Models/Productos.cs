namespace BackEnd.Models
{
    public class Productos
    {
        public int ProId { get; set; }
        public int CatId { get; set; }
        public int? EstId { get; set; }
        public string ProSexo { get; set; } = "UniSex";
        public string ProNombre { get; set; } = string.Empty;
        public string? ProDescripcion {  get; set; }
        public short? ProDescuento { get; set; }
        public DateTime? EstDescuentoInicio { get; set; }
        public DateTime? EstDescuentoFin {  get; set; }
        public DateTime? EstFechaCreacion { get; set; }

        //Navigation Properties
        public Categorias Categoria { get; set; } = null!; //Un producto pertenece a una categoria
        public Estilos? Estilo { get; set; } //Un producto puede pertenecer a un estilo
        public ICollection<Variantes> Variantes { get; set; } = new List<Variantes>();
        
    }
}
