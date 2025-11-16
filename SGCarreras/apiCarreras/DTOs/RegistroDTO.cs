using static apiCarreras.Controllers.CarrerasSimuladasController;

namespace apiCarreras.DTOs
{
    public class RegistroDTO
    {
        public int Id { get; set; }
        public int NumeroEnCarrera { get; set; }
        public string tiempoperoenstingayuda { get; set; } = string.Empty;
        public double Distancia { get; set; }
        public int PosicionEnCarrera { get; set; }
        public TimeSpan Tiempo { get; set; }
        public DateTime HoraAvance { get; set; } = DateTime.Now;
        public int pntoControl { get; set; }
        public DateTime HoraDeFinalizacion { get; set; }
        public CorredorDTO Corredor { get; set; }

        public RegistroDTO() { }
    }
}
