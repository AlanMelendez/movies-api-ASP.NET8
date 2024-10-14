using APP_PELICULAS.Entities;
using Microsoft.EntityFrameworkCore;

namespace PeliculasAPI
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Genero> Generos { get; set; }
    }
}
