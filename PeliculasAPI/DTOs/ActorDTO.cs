namespace PeliculasAPI.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
    }
}
