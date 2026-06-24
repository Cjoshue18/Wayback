using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.ClientesVista
{
    public class UpsertDireccionesDTO
    {
        [Required]
        [MaxLength(100)]
        public string DirCalle { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DirDistrito { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DirProvincia { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DirDepartamento { get; set; } = string.Empty;

        [MaxLength(200)] //puedes decidir poner referencia o no
        public string? DirReferencia { get; set; } //puede ser nulo

        [Required] //tiene que ser o falso o verdadero
        public bool DirPreferido { get; set; } = false; //por defecto llega como falso
    }
}
