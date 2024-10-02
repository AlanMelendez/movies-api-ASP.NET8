using APP_PELICULAS.Entities;

namespace PeliculasAPI.Interfaces
{
    public interface IRepositoy
    {
        List<Genero> getAllGenders();
        Task<Genero?> getGenderById(int id);
        bool ExistsGender(string name);
    }
}
