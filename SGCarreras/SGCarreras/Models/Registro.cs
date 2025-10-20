using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class Registro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int numeroEnCarrera { get; set; }

        public int posicionEnCarrera { get; set; }

        public DateTime horaDeFinalizacion { get; set; }

        //Releishon Taim
        public int CorredorId { get; set; }
        public int CarreraId { get; set; }

        [ForeignKey("CorredorId")]
        public Corredor Corredor { get; set; }

        [ForeignKey("CarreraId")]
        public Carrera Carrera { get; set; }
        public ICollection<TiempoParcial> TiemposParciales { get; set; } 


        public Registro() { }

        public Registro(int numeroEnCarrera, int posicionEnCarrera, DateTime horaDeFinalizacion)
        {
            this.numeroEnCarrera = numeroEnCarrera;
            this.posicionEnCarrera = posicionEnCarrera;
            this.horaDeFinalizacion = horaDeFinalizacion;
        }
    }
}



    
