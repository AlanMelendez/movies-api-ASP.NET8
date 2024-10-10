using APP_PELICULAS.Entities;
using APP_PELICULAS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private const string cacheTag = "genders";
        private readonly IRepositoy _repositoryInMemory;
        private readonly IOutputCacheStore _outputCacheStore;

        public GenerosController(IRepositoy repository, IOutputCacheStore outputCacheStore)
        {
            _repositoryInMemory = repository;
            _outputCacheStore = outputCacheStore;
        }

        [HttpGet("{id}")]
        [OutputCache(Tags =[cacheTag])]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _repositoryInMemory.getGenderById(id);
            if (genero == null)
            {
                return NotFound();
            }
            return genero;
        }


        [HttpGet("Listado")]
        [OutputCache(Tags = [cacheTag])]

        public IEnumerable<Genero> GetGeneros()
        {
            var generos = _repositoryInMemory.getAllGenders();

            return generos;
            //return _repositoryInMemory.getAllGenders();
        }

        [HttpPost]
        [OutputCache(Tags = [cacheTag])]
        public async Task< IActionResult> Post([FromBody] Genero genero)
        {
             var existGenderWithDealName= _repositoryInMemory.ExistsGender(genero.Nombre);

            if (existGenderWithDealName) { 
                return BadRequest($"If exist gender with this name {genero.Nombre}");
            }

            await _outputCacheStore.EvictByTagAsync(cacheTag, default);

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