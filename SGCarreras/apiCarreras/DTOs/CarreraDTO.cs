namespace apiCarreras.DTOs
{
    public class CarreraDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public DateTime? HoraInicio { get; set; }
        public List<PuntoDeControlDTO> PtosDeControl { get; set; } = new();
        public List<RegistroDTO> Registros { get; set; } = new();

    }
}
