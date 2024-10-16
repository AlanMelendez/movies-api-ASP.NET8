namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        public int _pagina { get; set; } = 1;
        private int _recordsPorPagina = 10;
        private readonly int _maxRecordsPorPagina = 50;
        public int RecordsPorPagina
        {
            get
            {
                return _recordsPorPagina;
            }
            set
            {
                _recordsPorPagina = (value > _maxRecordsPorPagina) ? _maxRecordsPorPagina : value;
            }
        }

    }
}
