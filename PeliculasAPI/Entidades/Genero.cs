using System.ComponentModel.DataAnnotations;
using PeliculasAPI.Validations;

namespace APP_PELICULAS.Entities
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [FirstCapitalLetter]
        public required string Nombre { get; set; }
    }
}