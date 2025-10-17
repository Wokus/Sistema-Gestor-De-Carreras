using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string nombreCompleto { get; set; }

        public string cedula { get; set; }

     
        public Usuario() { }

        public Usuario( string nombreCompleto, string cedula)
        {
            this.nombreCompleto = nombreCompleto;
            this.cedula = cedula;
        }
    }
}



    
