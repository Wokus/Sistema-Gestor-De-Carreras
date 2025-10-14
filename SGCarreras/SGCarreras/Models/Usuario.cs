namespace SGCarreras.Models
{
    public class Usuario
    {
        public string id { get; set; }

        public string nombreCompleto { get; set; }

        public string cedula { get; set; }

     
        public Usuario() { }

        public Usuario(string id, string nombreCompleto, string cedula)
        {
            this.id = id;
            this.nombreCompleto = nombreCompleto;
            this.cedula = cedula;
        }
    }
}



    
