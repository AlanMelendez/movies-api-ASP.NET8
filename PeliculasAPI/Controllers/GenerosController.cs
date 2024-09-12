using APP_PELICULAS.Entities;
using APP_PELICULAS.Services;
using Microsoft.AspNetCore.Mvc;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
          private readonly RepositoryInMemory _repositoryInMemory;

        public GenerosController()
        {
            _repositoryInMemory = new RepositoryInMemory();
        }


        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Get controllers activated");
        }

        [HttpGet("Listado")]
        public IEnumerable<Genero> GetGeneros()
        {
            var repository = new RepositoryInMemory();
            var generos = repository.getAllGenders();

            return generos;
            //return _repositoryInMemory.getAllGenders();
        }

    }
}