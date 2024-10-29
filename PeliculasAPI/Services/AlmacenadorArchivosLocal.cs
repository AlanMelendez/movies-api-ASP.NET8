
namespace PeliculasAPI.Services
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {

            var extImg = Path.GetExtension(archivo.FileName);
            var nameImg = $"{Guid.NewGuid()}{extImg}";
            string folder = Path.Combine(_env.WebRootPath, contenedor);

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, nameImg);

            //Save img
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(path, content);

            }

            // Path where the image is stored
            var request = _httpContextAccessor.HttpContext!.Request;
            var host = $"{request.Scheme}://{request.Host}";
            var urlFile = Path.Combine(host, contenedor, nameImg).Replace("\\", "/");

            return urlFile;
        }

        public Task Borrar(string? ruta, string contenedor)
        {
            if(string.IsNullOrEmpty(ruta))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(ruta);
            string folderFile = Path.Combine(_env.WebRootPath, contenedor, fileName);

            if (File.Exists(folderFile))
            {
                File.Delete(folderFile);
            }
            return Task.CompletedTask;


        }
    }
}
