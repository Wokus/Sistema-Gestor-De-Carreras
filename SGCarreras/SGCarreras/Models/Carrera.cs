using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using static SGCarreras.Models.Estado;

namespace SGCarreras.Models
{
    public class Carrera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string nombre { get; set; }

        public string ubicacion { get; set; }

        public EstadoEnum estado { get; set; }

        public double kmTotales { get; set; }


        //Relaciones

        public ICollection<Registro> registros { get; set; } = new List<Registro>();
        public ICollection<PuntoDeControl> PuntosDeControl { get; set; }


        public Carrera() { }

        public Carrera(string nombre, string ubicacion, EstadoEnum estado, double kmTotales)
        {
            this.nombre = nombre;
            this.ubicacion = ubicacion;
            this.estado = estado;  
            this.kmTotales = kmTotales;
        }
    }
}



    
