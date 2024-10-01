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

        public GenerosController(RepositoryInMemory repository)
        {
            _repositoryInMemory = repository;
        }


        [HttpGet("Listado")]
        public IEnumerable<Genero> GetGeneros()
        {
            var generos = _repositoryInMemory.getAllGenders();

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
        public IActionResult Post([FromBody] Genero genero)
        {
             var existGenderWithDealName= _repositoryInMemory.ExistsGender(genero.Nombre);

            if (existGenderWithDealName) { 
                return BadRequest($"If exist gender with this name {genero.Nombre}");
            }

            return Ok();           
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