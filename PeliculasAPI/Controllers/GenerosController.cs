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
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly ApplicationDBContext _applicationDBContext;

        public GenerosController(IOutputCacheStore outputCacheStore, ApplicationDBContext applicationDBContext)
        {
            _outputCacheStore = outputCacheStore;
            _applicationDBContext = applicationDBContext;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public List<Genero> Get()
        {
            return new List<Genero>()
            {
                new Genero{Id=1, Nombre="Comedia"},
                new Genero{Id=2, Nombre="Accion"}
            };
        }

        [HttpGet("{id}", Name = "ObtenerGeneroPorID")]
        [OutputCache(Tags =[cacheTag])]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        [OutputCache(Tags = [cacheTag])]
        public async Task< IActionResult> Post([FromBody] Genero genero)
        {
           _applicationDBContext.Add(genero);
            await _applicationDBContext.SaveChangesAsync();
            return CreatedAtRoute("ObtenerGeneroPorID", new {id = genero.Id}, genero);

        }

        [HttpPut]
        public void Put()
        {
            //return Ok("Put controllers activated");
            throw new NotImplementedException();

        }

        [HttpDelete]
        public void Delete()
        {
            //return Ok("Delete controllers activated");
            throw new NotImplementedException();

        }

    }
}