using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Utilidades;
using System.Linq.Expressions;

namespace PeliculasAPI.Controllers
{
    public class CustomBaseController: ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public CustomBaseController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TDTO>> Get<TEntidad, TDTO>
        (PaginacionDTO paginacion, Expression<Func<TEntidad, object>> ordenarPor) where TEntidad : class
        {
           var queryable = _context.Set<TEntidad>().AsQueryable();
            await HttpContext.InsertarParametrosPaginacionCabecera(queryable);
            return await queryable
                .OrderBy(ordenarPor)
                .Paginar(paginacion)
                .ProjectTo<TDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        }


    }
}
