using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class PuntoDeControl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public double Distancia { get; set; }

        //Relacioneison
        public int CarreraId { get; set; }

        [Required]
        [ForeignKey("CarreraId")]
        public Carrera? Carrera { get; set; }
        public PuntoDeControl() { }

        public PuntoDeControl( double distancia)
        {
            this.Distancia = distancia;
        }
    }
}
