using static SGCarreras.Models.Sexo;

namespace SGCarreras.Models
{
    public class Corredor
    {
        public SexoEnum sexo { get; set; }
        string nacionalidad { get; set; }

        //Relaciones
        public List<Registro> registros { get; set; } = new();

        public Corredor() { }

        public Corredor(SexoEnum sexo, string nacionalidad)
        {
            this.sexo = sexo;
            this.nacionalidad = nacionalidad;
        }
    }
}



    
