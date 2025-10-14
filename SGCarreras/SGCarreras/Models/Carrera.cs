using System.Security.Cryptography.X509Certificates;
using static SGCarreras.Models.Estado;

namespace SGCarreras.Models
{
    public class Carrera
    {
        public string id { get; set; }

        public string nombre { get; set; }

        public string ubicacion { get; set; }

        public EstadoEnum estado { get; set; }

        public double kmTotales { get; set; }


        public Carrera() { }

        public Carrera(string id, string nombre, string ubicacion, EstadoEnum estado, double kmTotales)
        {
            this.id = id;
            this.nombre = nombre;
            this.ubicacion = ubicacion;
            this.estado = estado;  
            this.kmTotales = kmTotales;
        }
    }
}



    
