using APP_PELICULAS.Services;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.Interfaces;
using PeliculasAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

//Mapeamos los controladores, para poder crear nuestra carpeta y controladores. (1)
builder.Services.AddControllers();

//Agregamos el servicio de cache para las peticiones.
builder.Services.AddOutputCache(
    options =>
    {
        options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
    }    
);

var originsAllowed = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(",");

builder.Services.AddCors(
    (opciones) =>{
        opciones.AddDefaultPolicy(
            (opcionesCORS) => { 
                opcionesCORS.WithOrigins(originsAllowed).AllowAnyMethod().AllowAnyHeader()
                .WithExposedHeaders("cantidad-total-registros"); 
            }
        );
    }
 );

// Add DBContext
builder.Services.AddDbContext<ApplicationDBContext>(
    options => options.UseSqlServer("name=DefaultConnection")
 );


// Save images in local
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();

// Other services to save images in Local
builder.Services.AddHttpContextAccessor(); // It is necessary to use the IHttpContextAccessor interface in the AlmacenadorArchivosLocal service.

//Azure services to storage image
//builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();

// For inject dependency
builder.Services.AddSingleton<IRepositoy ,RepositoryInMemory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseOutputCache(); //Agregamos el servicio de cache para las peticiones.
app.UseStaticFiles(); //Middleware to serve static files

app.MapControllers(); //Mapeamos los controladores, para poder crear nuestra carpeta y controladores. (2)
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
