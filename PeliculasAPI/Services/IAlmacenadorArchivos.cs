namespace PeliculasAPI.Services
{
    public interface IAlmacenadorArchivos
    {
        Task<string> Almacenar(string contenedor, IFormFile archivo);
        Task Borrar(string? ruta, string contenedor);
        async Task<string> Editar(string contenedor, IFormFile archivo, string? ruta)
        {
            await Borrar(ruta, contenedor);
            return await Almacenar(contenedor, archivo);
        }
    }
}
