
# Documentación de la Función `Get` en CustomBaseController

## Descripción General

La función `Get` en el controlador `CustomBaseController` es un método genérico diseñado para manejar de forma flexible 
y eficiente la obtención de datos desde la base de datos, permitiendo aplicar:
- **Ordenamiento** dinámico de resultados según un criterio especificado.
- **Paginación** de resultados para mejorar la gestión y visualización de grandes conjuntos de datos.
- **Proyección** de entidades de la base de datos en DTOs (Data Transfer Objects) utilizando AutoMapper, optimizando así 
  la transferencia de datos a través de la API.

## Firma de la Función

```csharp
public async Task<List<TDTO>> Get<TEntidad, TDTO>
(PaginacionDTO paginacion, Expression<Func<TEntidad, object>> ordenarPor) where TEntidad : class
```

## Parámetros de Entrada

- `PaginacionDTO paginacion`: Instancia de la clase `PaginacionDTO` que contiene la información necesaria para realizar 
  la paginación de resultados. Suele incluir el número de página y el tamaño de cada página.
  
- `Expression<Func<TEntidad, object>> ordenarPor`: Expresión lambda que define el criterio de ordenamiento de los 
  resultados. Por ejemplo, `x => x.Nombre` ordenaría los resultados por el campo `Nombre`.

## Explicación Paso a Paso

1. **Creación de la Consulta (`Queryable`)**:
   ```csharp
   var queryable = _context.Set<TEntidad>().AsQueryable();
   ```
   Utiliza el contexto de la base de datos para crear una consulta `IQueryable` sobre el tipo `TEntidad`, que es 
   genérico y representa cualquier entidad de la base de datos. Esto permite aplicar filtros y operaciones sin cargar 
   todos los datos en memoria, optimizando así la consulta.

2. **Inserción de Parámetros de Paginación en el Encabezado de la Respuesta HTTP**:
   ```csharp
   await HttpContext.InsertarParametrosPaginacionCabecera(queryable);
   ```
   `InsertarParametrosPaginacionCabecera` es un método auxiliar que agrega información sobre la paginación 
   (como el total de elementos y el número de páginas) en los encabezados de la respuesta HTTP. Esto es útil para que 
   el cliente pueda gestionar mejor la visualización de los resultados.

3. **Ordenamiento, Paginación y Proyección**:
   ```csharp
   return await queryable
       .OrderBy(ordenarPor)
       .Paginar(paginacion)
       .ProjectTo<TDTO>(_mapper.ConfigurationProvider)
       .ToListAsync();
   ```
   - **Ordenamiento**: `OrderBy(ordenarPor)` utiliza la expresión `ordenarPor` para ordenar la consulta según el criterio 
     especificado.
   - **Paginación**: `Paginar(paginacion)` aplica la paginación usando los datos en `paginacion`, limitando la cantidad 
     de resultados devueltos en cada consulta y mejorando la eficiencia.
   - **Proyección con AutoMapper**: `ProjectTo<TDTO>(_mapper.ConfigurationProvider)` convierte cada instancia de 
     `TEntidad` en un objeto `TDTO` utilizando AutoMapper, reduciendo los datos innecesarios transferidos en la respuesta.
   - **Ejecución Asíncrona**: `ToListAsync()` ejecuta la consulta en la base de datos y obtiene los resultados como una 
     lista de `TDTO`, de forma asíncrona.

## Ejemplo de Uso

Aquí tienes un ejemplo de cómo podrías invocar esta función en un controlador que herede de `CustomBaseController`:

```csharp
public class PeliculasController : CustomBaseController
{
    public PeliculasController(ApplicationDBContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<IActionResult> ObtenerPeliculas([FromQuery] PaginacionDTO paginacion)
    {
        var resultado = await Get<Pelicula, PeliculaDTO>(paginacion, ordenarPor: x => x.Titulo);
        return Ok(resultado);
    }
}
```

En este ejemplo, `Get` se usa para obtener una lista paginada de películas, ordenadas por su título y proyectadas en `PeliculaDTO`.

## Beneficios

- **Reutilización**: Al ser un método genérico, puede usarse en múltiples entidades y DTOs, siguiendo el principio de DRY.
- **Optimización de Rendimiento**: La consulta se ejecuta en la base de datos, evitando cargar datos innecesarios en 
  memoria.
- **Flexibilidad**: Permite especificar tanto el criterio de ordenamiento como los detalles de paginación, ofreciendo 
  una gran flexibilidad para consultas complejas.

## Requisitos

- **AutoMapper**: Se necesita AutoMapper para la proyección a DTO.
- **Entity Framework Core**: Es necesario para ejecutar consultas LINQ en la base de datos.


#
# Explicación Detallada de Conceptos Clave en CustomBaseController


Este apartado profundiza en los conceptos importantes usados en `CustomBaseController`, en particular, sobre cómo se implementa la herencia, el uso de `IQueryable`, `Expression<Func<T, object>>`, `ProjectTo`, y el concepto de asynchrony.

## 1. Herencia y DI (Inyección de Dependencias)

En el ejemplo de uso:
```csharp
public class PeliculasController : CustomBaseController
{
    public PeliculasController(ApplicationDBContext context, IMapper mapper) : base(context, mapper) { }
}
```

### ¿Por qué se hereda `CustomBaseController`?

La herencia permite reutilizar la lógica común de consulta de `Get` para distintas entidades, facilitando que los 
controladores puedan realizar consultas y paginación sin repetir código.

### ¿Por qué se pasa `context` y `mapper`?

1. `ApplicationDBContext context`: representa el contexto de la base de datos, que permite realizar consultas y 
   transacciones. Necesitamos `context` para acceder a los datos en `CustomBaseController`.

2. `IMapper mapper`: AutoMapper permite proyectar entidades en DTOs para que no se exponga la estructura de la base de 
   datos directamente a la API, mejorando la seguridad y la eficiencia en la transferencia de datos.

## 2. Expresiones Lambda (`Expression<Func<TEntidad, object>> ordenarPor`)

En `Get`, usamos un parámetro lambda para especificar el campo de ordenamiento dinámicamente:
```csharp
Expression<Func<TEntidad, object>> ordenarPor
```

### ¿Qué es `Expression<Func<TEntidad, object>>`?

Este tipo permite que el código reciba una expresión lambda (como `x => x.Nombre`) en lugar de valores concretos. 
El tipo `Expression` permite que la lambda se traduzca directamente en una instrucción SQL que puede ejecutarse en 
la base de datos, optimizando la consulta.

### ¿Por qué es útil?

Con una expresión lambda, el controlador no necesita saber de antemano por cuál campo ordenar, sino que permite que 
la ordenación sea flexible. Ejemplo:
```csharp
Get<Pelicula, PeliculaDTO>(paginacion, x => x.Titulo);
```
Este ejemplo ordena la consulta por el campo `Titulo`.

## 3. `AsQueryable`

Cuando usamos `AsQueryable`:
```csharp
var queryable = _context.Set<TEntidad>().AsQueryable();
```

### ¿Qué es `AsQueryable` y por qué se usa?

`AsQueryable` convierte una colección en `IQueryable`, que permite realizar consultas en modo diferido. Esto significa que 
no se ejecuta la consulta hasta que realmente necesitemos los datos (por ejemplo, cuando llamamos a `ToListAsync`). 
Permite aplicar filtros, ordenamientos y paginaciones que luego se traducen en SQL, optimizando el rendimiento.

### Diferencia con `IEnumerable`

`IQueryable` permite que toda la consulta se ejecute en la base de datos, mientras que `IEnumerable` carga los datos 
en memoria y luego aplica las operaciones. `IQueryable` es más eficiente para trabajar con grandes volúmenes de datos.

## 4. `ProjectTo` y el Uso de AutoMapper

En la línea:
```csharp
.ProjectTo<TDTO>(_mapper.ConfigurationProvider)
```

### ¿Qué es `ProjectTo`?

`ProjectTo` usa AutoMapper para convertir automáticamente una entidad de la base de datos (`TEntidad`) en su correspondiente 
DTO (`TDTO`). 

- **Ventaja**: No necesitas hacer manualmente la conversión, y solo se seleccionan los campos definidos en el DTO, 
  reduciendo la cantidad de datos transferidos y mejorando el rendimiento.
- **Uso de `_mapper.ConfigurationProvider`**: Define las reglas de mapeo entre la entidad y el DTO, configuradas 
  en AutoMapper.

### Ejemplo

Si tenemos:
```csharp
public class PeliculaDTO { public string Titulo { get; set; } }
```

Al hacer `ProjectTo<PeliculaDTO>`, AutoMapper selecciona solo el campo `Titulo`, evitando transferir toda la información 
de la entidad `Pelicula`.

## 5. Ejecución Asíncrona y `ToListAsync`

La función termina con:
```csharp
.ToListAsync();
```

### ¿Qué hace `ToListAsync`?

`ToListAsync` ejecuta la consulta de forma asíncrona. En lugar de bloquear el hilo hasta que la consulta termine, `ToListAsync` 
permite que el proceso continúe y maneje otros trabajos en paralelo. Esto mejora el rendimiento en aplicaciones con múltiples usuarios.

- **Ventaja de la asincronía**: Mejora la capacidad de respuesta de la API, permitiendo manejar más solicitudes concurrentes.

## Resumen y Beneficios Clave

- **Herencia y DI**: Permite reutilizar `context` y `mapper` en otros controladores.
- **Lambda y Expresiones**: Proporcionan flexibilidad para ordenar por diferentes campos sin alterar el código.
- **AsQueryable**: Optimiza la consulta para que se ejecute en la base de datos.
- **ProjectTo y AutoMapper**: Facilita la conversión a DTOs, mejorando seguridad y rendimiento.
- **Ejecución Asíncrona**: Permite un uso más eficiente de los recursos del servidor.

Este enfoque hace que `CustomBaseController` sea una base extensible y optimizada para consultas en una API.

