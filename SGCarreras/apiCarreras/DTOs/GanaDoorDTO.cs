using static apiCarreras.Controllers.CarrerasSimuladasController;

namespace apiCarreras.DTOs
{
    public class GanaDoorDTO
    {
        public int registroId {  get; set; }
        public string tiempoDeFinalizacion { get; set; }
        public int numeroEnCarrera { get; set; }

        public int numeroDelCorredor { get; set; }

        public GanaDoorDTO() { }
    }
}
