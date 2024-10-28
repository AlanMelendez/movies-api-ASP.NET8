using APP_PELICULAS.Entities;
using AutoMapper;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeoGeneros();
            ConfigurarMapeoActores();
        }

        public void ConfigurarMapeoGeneros()
        {
               CreateMap<GeneroCreacionDTO, GeneroDTO>();
               CreateMap<Genero, GeneroDTO>();
               CreateMap<GeneroCreacionDTO, Genero>();
        }

        private void ConfigurarMapeoActores()
        {
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore()); //Ignoramos la foto ya que no se mapea.
        }
    }
}
