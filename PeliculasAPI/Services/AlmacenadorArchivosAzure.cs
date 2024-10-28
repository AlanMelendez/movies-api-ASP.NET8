
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace PeliculasAPI.Services
{
    public class AlmacenadorArchivosAzure : IAlmacenadorArchivos
    {
        private string connectionString;

        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
        }

        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            // Configura las opciones del cliente para establecer una versión específica del header, error: {"The value for one of the HTTP headers is not in the correct format.\nRequestId:0ef1e088-50}
            var blobClientOptions = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2024_11_04);


            var cliente = new BlobContainerClient(connectionString, contenedor,blobClientOptions);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var extensionArchivo = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extensionArchivo}"; //Name of the file is a GUID (aleatory)
            var blob = cliente.GetBlobClient(nombreArchivo);

            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = archivo.ContentType;
            //blobHttpHeaders.ContentType = archivo.ContentType;
            //x-ms-request-id: c63d286a-701e-0101-5df6-2844ab000000

            await blob.UploadAsync(archivo.OpenReadStream(), blobHttpHeaders);
            return blob.Uri.ToString();
        }

        public async Task Borrar(string? ruta, string contenedor)
        {
           if(string.IsNullOrWhiteSpace(ruta))
            {
                return;
            }
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();

            var nombreArchivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(nombreArchivo);
            await blob.DeleteIfExistsAsync();

        }
    }
}
