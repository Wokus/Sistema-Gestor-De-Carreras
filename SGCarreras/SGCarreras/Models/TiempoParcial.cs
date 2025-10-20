using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class TiempoParcial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int tiempo { get; set; }


        //Relaciones Tiempo de:
        public int PuntoControlId { get; set; }
        public int RegistroId { get; set; }

        [ForeignKey("PuntoControlId")]
        public PuntoDeControl PuntoControl { get; set; }

        [ForeignKey("RegistroId")]
        public Registro Registro { get; set; }

        public TiempoParcial() { }

        public TiempoParcial(int tiempo)
        {
            this.tiempo = tiempo;
        }
    }
}



    
