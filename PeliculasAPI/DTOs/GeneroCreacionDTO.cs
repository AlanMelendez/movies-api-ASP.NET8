using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class GeneroCreacionDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe ser menor a 50 caracteres.")]
        [FirstCapitalLetter]
        public required string Nombre { get; set; }
    }
}
