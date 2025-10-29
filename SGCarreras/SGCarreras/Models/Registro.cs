using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class Registro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NumeroEnCarrera { get; set; }

        public int PosicionEnCarrera { get; set; }

        public DateTime HoraDeFinalizacion { get; set; }

        //Releishon Taim
        public int CorredorId { get; set; }
        public int CarreraId { get; set; }

        [Required]
        [ForeignKey("CorredorId")]
        public Corredor? Corredor { get; set; }

        [Required]
        [ForeignKey("CarreraId")]
        public Carrera? Carrera { get; set; }
        public ICollection<TiempoParcial> TiemposParciales { get; set; } = [];

        public Registro() { }

        public Registro(int numeroEnCarrera, int posicionEnCarrera, DateTime horaDeFinalizacion)
        {
            this.NumeroEnCarrera = numeroEnCarrera;
            this.PosicionEnCarrera = posicionEnCarrera;
            this.HoraDeFinalizacion = horaDeFinalizacion;
        }
    }
}
