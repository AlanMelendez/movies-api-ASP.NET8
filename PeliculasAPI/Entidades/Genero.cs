using System.ComponentModel.DataAnnotations;
using PeliculasAPI.Validations;

namespace APP_PELICULAS.Entities
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe ser menor a 50 caracteres.")]
        [FirstCapitalLetter]
        public required string Nombre { get; set; }
    }
}