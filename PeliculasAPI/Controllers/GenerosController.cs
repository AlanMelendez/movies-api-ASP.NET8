using Microsoft.AspNetCore.Mvc;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        public GenerosController()
        {

        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Lista de géneros de películas");
        }

        [HttpGet("ObtenerGeneros")]
        public ActionResult Get(int id)
        {
            return Ok($"Género con id {id}");
        }
    }
}