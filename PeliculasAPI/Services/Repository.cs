using APP_PELICULAS.Entities;
using PeliculasAPI.Interfaces;

namespace APP_PELICULAS.Services
{
    public class RepositoryInMemory: IRepositoy
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

        public bool ExistsGender(string name)
        {
            return _generos.Any(gender=> gender.Nombre == name);
        }


        public async Task<Genero?> getGenderById(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return _generos.FirstOrDefault(x => x.Id == id); //return gender or null
        }
    }
}