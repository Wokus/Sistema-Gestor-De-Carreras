using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class TiempoParcial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Tiempo { get; set; }

        //Relaciones Tiempo de:
        public int PuntoControlId { get; set; }
        public int RegistroId { get; set; }

        [Required]
        [ForeignKey("PuntoControlId")]
        public PuntoDeControl? PuntoControl { get; set; }

        [Required]
        [ForeignKey("RegistroId")]
        public Registro? Registro { get; set; }

        public TiempoParcial() { }

        public TiempoParcial(int tiempo)
        {
            this.Tiempo = tiempo;
        }
    }
}
