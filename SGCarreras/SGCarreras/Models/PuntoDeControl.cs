using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [ForeignKey("CarreraId")]
        [JsonIgnore]
        public Carrera? Carrera { get; set; }
        public PuntoDeControl() { }

        public PuntoDeControl( double distancia)
        {
            this.Distancia = distancia;
        }
    }
}
