using System.ComponentModel.DataAnnotations;

namespace APP_PELICULAS.Entities
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public required string Nombre { get; set; }
    }
}