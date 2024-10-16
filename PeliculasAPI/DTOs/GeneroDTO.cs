using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class GeneroDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}
