using APP_PELICULAS.Entities;
using APP_PELICULAS.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Interfaces;
using PeliculasAPI.Utilidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private const string cacheTag = "genders";
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IMapper _mapper;

        public GenerosController(IOutputCacheStore outputCacheStore, ApplicationDBContext applicationDBContext, IMapper mapper)
        {
            _outputCacheStore = outputCacheStore;
            _applicationDBContext = applicationDBContext;
            _mapper = mapper;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<GeneroDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = _applicationDBContext.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionCabecera(queryable);

            return await  queryable
                          .OrderBy(x => x.Nombre)
                          .Paginar(paginacion)
                          .ProjectTo<GeneroDTO>(_mapper.ConfigurationProvider)
                          .ToListAsync();


            //var generos = await _applicationDBContext.Generos.ToListAsync();
            //var generosDTO = _mapper.Map<List<GeneroDTO>>(generos); 

            //return generosDTO; //Recibimos GeneroDTO, Buscamos generos en la base de datos y los mapeamos a GeneroDTO.
        }

        [HttpGet("{id}", Name = "ObtenerGeneroPorID")]
        [OutputCache(Tags =[cacheTag])]
        public async Task<ActionResult<GeneroDTO>> GetGenero(int id)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        [OutputCache(Tags = [cacheTag])]
        public async Task< IActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _mapper.Map<Genero>(generoCreacionDTO);
            _applicationDBContext.Add(genero);
            await _applicationDBContext.SaveChangesAsync();
            return CreatedAtRoute("ObtenerGeneroPorID", new {id = generoCreacionDTO.Id}, generoCreacionDTO);

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