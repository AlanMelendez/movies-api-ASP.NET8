using Microsoft.AspNetCore.Mvc;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReglasController : ControllerBase
    {
        // Regla 1: Ruta predeterminada
        // Esta ruta sigue el patrón predeterminado: {controller}/{action}/{id?}
        // Si accedes a: /Reglas/Index o simplemente /Reglas, caerá en esta acción.
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Ruta predeterminada - Index");
        }

        // Regla 2: Ruta personalizada con un parámetro opcional
        // Esta regla establece un parámetro llamado "id" que es opcional.
        // Ejemplo: /Reglas/Detalle/123 o /Reglas/Detalle
        [HttpGet("Detalle/{id?}")]
        public IActionResult Detalle(int? id)
        {
            if (id.HasValue)
                return Ok($"Detalle con ID: {id}");
            else
                return Ok("Detalle sin ID especificado");
        }

        // Regla 3: Ruta con restricción de parámetros
        // El parámetro "id" debe ser un número (con restricción: int).
        // Ejemplo: /Reglas/Producto/123 funcionará, pero /Reglas/Producto/abc dará error.
        [HttpGet("Producto/{id:int}")]
        public IActionResult Producto(int id)
        {
            return Ok($"Producto con ID: {id}");
        }

        // Regla 4: Ruta con prefijo de texto estático
        // En este caso, la ruta requiere que la URL comience con "info".
        // Ejemplo: /Reglas/info/Detalles
        [HttpGet("info/Detalles")]
        public IActionResult InfoDetalles()
        {
            return Ok("Detalles de la información");
        }

        // Regla 5: Ruta con varios parámetros
        // Puedes incluir múltiples parámetros en la ruta.
        // Ejemplo: /Reglas/Buscar?nombre=juan&edad=25
        [HttpGet("Buscar")]
        public IActionResult Buscar([FromQuery] string nombre, [FromQuery] int edad)
        {
            return Ok($"Nombre: {nombre}, Edad: {edad}");
        }

        // Regla 6: Ruta con prefijo y sufijo opcional
        // Aquí puedes definir partes opcionales de la URL. 
        // Ejemplo: /Reglas/Ver/2023/08 funcionará, pero también /Reglas/Ver/2023.
        [HttpGet("Ver/{anio}/{mes?}")]
        public IActionResult Ver(int anio, int? mes)
        {
            if (mes.HasValue)
                return Ok($"Año: {anio}, Mes: {mes}");
            else
                return Ok($"Año: {anio}, sin mes especificado");
        }

        // Regla 7: Ruta para múltiples verbos HTTP
        // La acción soporta tanto GET como POST.
        // Ejemplo: /Reglas/Crear desde un formulario usando POST o GET.
        [HttpGet("Crear")]
        [HttpPost("Crear")]
        public IActionResult Crear()
        {
            return Ok("Crear acción usando GET o POST");
        }

        // Regla 8: Ruta con valor por defecto en el parámetro
        // El parámetro "tipo" tiene un valor por defecto si no se especifica.
        // Ejemplo: /Reglas/Filtrar o /Reglas/Filtrar/Avanzado
        [HttpGet("Filtrar/{tipo=Basico}")]
        public IActionResult Filtrar(string tipo)
        {
            return Ok($"Filtrado usando tipo: {tipo}");
        }

        // Regla 9: Ruta con expresión regular para parámetros
        // El parámetro "codigo" debe seguir una expresión regular, en este caso 3 letras seguidas por 3 dígitos.
        // Ejemplo: /Reglas/Codigo/ABC123 será válido, pero /Reglas/Codigo/123ABC no lo será.
        // [HttpGet("Codigo/{codigo:regex(^[A-Z]{3}[0-9]{3}$)}")]
        // public IActionResult Codigo(string codigo)
        // {
        //     return Ok($"Código válido: {codigo}");
        // }

        // Regla 10: Ruta con una lista de parámetros permitidos
        // Esta ruta solo permite ciertos valores específicos para el parámetro "categoria".
        // Ejemplo: /Reglas/Categoria/Deportes o /Reglas/Categoria/Tecnologia
        [HttpGet("Categoria/{categoria:alpha:minlength(4)}")]
        public IActionResult Categoria(string categoria)
        {
            if (categoria == "Deportes" || categoria == "Tecnologia" || categoria == "Musica")
                return Ok($"Categoría válida: {categoria}");
            else
                return BadRequest("Categoría no válida");
        }
    }
}
