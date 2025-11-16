namespace apiCarreras.DTOs
{
   public class CarreraDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public DateTime HoraInicio { get; set; }
        public List<PuntoDeControlDTO> PuntosDeControl { get; set; } = new();
        public List<RegistroDTO> Inscripciones { get; set; } = new();

    }
}
