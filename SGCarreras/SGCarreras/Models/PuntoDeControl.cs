using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class PuntoDeControl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  id { get; set; }

        public double distancia { get; set; }

        //Relacioneison
        public int CarreraId { get; set; }

        [ForeignKey("CarreraId")]
        public Carrera Carrera { get; set; }
        public PuntoDeControl() { }

        public PuntoDeControl( double distancia)
        {
            this.distancia = distancia;
        }
    }
}



    
