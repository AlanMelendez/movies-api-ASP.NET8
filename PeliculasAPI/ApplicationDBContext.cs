using APP_PELICULAS.Entities;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;

namespace PeliculasAPI
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        //Agregamos las tablas a la base de datos.
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }









        /*
            Comando para crear las migraciones:
            Add-Migration "NombreMigracion" Ex: Add-Migration TablaActores

            Comando para actualizar la base de datos:
            Update-Database

            Comando para borrar la ultima migracion:
            Remove-Migration

            Comando para borrar la base de datos:
            Drop-Database

            Comando para borrar la base de datos y crearla nuevamente:
            Update-Database -Migration 0


         
         */

    }
}
