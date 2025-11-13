using static apiCarreras.Controllers.CarrerasSimuladasController;

namespace apiCarreras.DTOs
{
    public class RegistroDTO
    {
        public int Id { get; set; }
        public int NumeroEnCarrera { get; set; }
        public int Distancia { get; set; }
        public int PosicionEnCarrera { get; set; }
        public TimeSpan? Tiempo { get; set; }
        public DateTime? HoraAvance { get; set; } = DateTime.Now;
        public int PuntoControl { get; set; }
        public DateTime HoraDeFinalizacion { get; set; }
        public required CorredorDTO Corredor { get; set; }
    }
}
