using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class ActorCreacionDTO
    {

        //public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public required string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
