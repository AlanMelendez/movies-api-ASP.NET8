namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        public int pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int _maxRecordsPorPagina = 50;
        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > _maxRecordsPorPagina) ? _maxRecordsPorPagina : value;
            }
        }

    }
}
