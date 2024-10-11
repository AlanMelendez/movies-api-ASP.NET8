using APP_PELICULAS.Services;
using PeliculasAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
                opcionesCORS.WithOrigins(originsAllowed).AllowAnyMethod().AllowAnyHeader(); 
            }
        );
    }
 );

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

app.MapControllers(); //Mapeamos los controladores, para poder crear nuestra carpeta y controladores. (2)
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
