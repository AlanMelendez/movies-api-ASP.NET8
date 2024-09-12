using APP_PELICULAS.Entities;

namespace APP_PELICULAS.Services
{
    public class RepositoryInMemory
    {
        public List<Genero> _generos { get; set; }
        public RepositoryInMemory()
        {
            _generos = new List<Genero>()
            {
                new Genero(){Id=1, Nombre="Comedia"},
                new Genero(){Id=2, Nombre="Aventura"},
                new Genero(){Id=3, Nombre="Acción"},
                new Genero(){Id=4, Nombre="Romance"},
                new Genero(){Id=5, Nombre="Suspenso"},
                new Genero(){Id=6, Nombre="Terror"},
            };
        }


        public List<Genero> getAllGenders()
        {
            return _generos;
        }
    }
}