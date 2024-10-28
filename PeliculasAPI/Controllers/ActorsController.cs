using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IOutputCacheStore _cache;
        private const string cacheTag = "actors";

        public ActorsController(ApplicationDBContext context, IMapper mapper, IOutputCacheStore cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }



        // GET: api/Actors/5
        [HttpGet("{id}", Name = "ObtenerActorPorID")]
        public void Get(int id)
        {
            throw new NotImplementedException();
        }
        //Para recibir la imagen y que el DTO la obtenga, es obligatorio usar el [FromForm]
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreacionDTO);

            //TODO: Pendiente trabajar la foto del actor.

            _context.Add(actor);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                return Ok( ex.Message);
            }
            await _cache.EvictByTagAsync(cacheTag, default);
            //return Ok(actor);

            return CreatedAtRoute("ObtenerActorPorID", new { id = actor.Id }, actor);


        }
    }
}
