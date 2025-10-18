using static SGCarreras.Models.Sexo;

namespace SGCarreras.Models
{
    public class Corredor : Usuario
    {
        public SexoEnum sexo { get; set; }
        string nacionalidad { get; set; }

        //Relaciones
        public ICollection<Registro> registros { get; set; }

        public Corredor() { }

        public Corredor(SexoEnum sexo, string nacionalidad)
        {
            this.sexo = sexo;
            this.nacionalidad = nacionalidad;
        }
    }
}



    
