using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Services;
using PeliculasAPI.Utilidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IOutputCacheStore _cache;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;


        private const string cacheTag = "actors";
        private readonly string contenedor = "actores";


        public ActorsController(ApplicationDBContext context, IMapper mapper, IOutputCacheStore cache, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ActorDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionCabecera(queryable);
            var actores = await queryable
                .OrderBy(x => x.Nombre)
                .Paginar(paginacion)
                .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return actores;
        }


        // GET: api/Actors/5
        [HttpGet("{id}", Name = "ObtenerActorPorID")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await _context.Actores
                        .AsQueryable()
                        .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return actor;
        }





        //Para recibir la imagen y que el DTO la obtenga, es obligatorio usar el [FromForm]
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreacionDTO);

            try
            {

                if (actorCreacionDTO.Foto != null)
                {
                    var urlFoto = await _almacenadorArchivos.Almacenar(contenedor, actorCreacionDTO.Foto);
                    actor.Foto = urlFoto;
                }

                _context.Add(actor);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
            await _cache.EvictByTagAsync(cacheTag, default);
            //return Ok(actor);

            return CreatedAtRoute("ObtenerActorPorID", new { id = actor.Id }, actor);


        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDB == null)
            {
                return NotFound();
            }
            actorDB = _mapper.Map(actorCreacionDTO, actorDB);
            if (actorCreacionDTO.Foto != null)
            {
                var urlFoto = await _almacenadorArchivos.Editar(contenedor, actorCreacionDTO.Foto, actorDB.Foto);
                actorDB.Foto = urlFoto;
            }
            await _context.SaveChangesAsync();
            await _cache.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registroBorrado = await _context.Actores.Where(x => x.Id == id).ExecuteDeleteAsync();

            if(registroBorrado == 0)
            {
                return NotFound();
            }

            await _cache.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
    }
}
