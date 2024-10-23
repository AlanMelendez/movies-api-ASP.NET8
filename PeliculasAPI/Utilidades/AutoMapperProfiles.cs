using APP_PELICULAS.Entities;
using AutoMapper;
using PeliculasAPI.DTOs;

namespace PeliculasAPI.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeoGeneros();
        }

        public void ConfigurarMapeoGeneros()
        {
               CreateMap<GeneroCreacionDTO, GeneroDTO>();
               CreateMap<Genero, GeneroDTO>();
               CreateMap<GeneroCreacionDTO, Genero>();


        }
    }
}
