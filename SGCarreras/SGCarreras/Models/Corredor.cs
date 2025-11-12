using System.Text.Json.Serialization;
using static SGCarreras.Models.Sexo;

namespace SGCarreras.Models
{
    public class Corredor : Usuario
    {
        public SexoEnum Sexo { get; set; }
        public string Nacionalidad { get; set; } = string.Empty;

        //Relaciones
        [JsonIgnore]
        public ICollection<Registro> registros { get; set; } = [];

        public Corredor() { }

        public Corredor(SexoEnum sexo, string nacionalidad)
        {
            Sexo = sexo;
            Nacionalidad = nacionalidad;
        }
    }
}
