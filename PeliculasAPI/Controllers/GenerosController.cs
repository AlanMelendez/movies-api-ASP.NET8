using APP_PELICULAS.Entities;
using APP_PELICULAS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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


        [HttpGet("Listado")]
        public IEnumerable<Genero> GetGeneros()
        {
            var repository = new RepositoryInMemory();
            var generos = repository.getAllGenders();

            return generos;
            //return _repositoryInMemory.getAllGenders();
        }



        [HttpGet("{id}")]
        [OutputCache]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _repositoryInMemory.getGenderById(id);
            if (genero == null)
            {
                return NotFound();
            }
            return genero;
        }


        [HttpPost]
        public void Post([FromBody] Genero genero)
        {
             Ok("Post controllers activated");
        }

        [HttpPut]
        public void Put()
        {
            //return Ok("Put controllers activated");
        }

        [HttpDelete]
        public void Delete()
        {
            //return Ok("Delete controllers activated");
        }

    }
}