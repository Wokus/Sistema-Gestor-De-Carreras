namespace SGCarreras.Models
{
    public class Registro
    {
        public int id { get; set; }
        public int numeroEnCarrera { get; set; }

        public int posicionEnCarrera { get; set; }

        public DateTime horaDeFinalizacion { get; set; }

        //Releishon Taim

        public Corredor Corredor { get; set; }
        public Carrera Carrera { get; set; }
        public List<TiempoParcial> TiemposParciales { get; set; } = new();


        public Registro() { }

        public Registro(int id, int numeroEnCarrera, int posicionEnCarrera, DateTime horaDeFinalizacion)
        {
            this.id = id;
            this.numeroEnCarrera = numeroEnCarrera;
            this.posicionEnCarrera = posicionEnCarrera;
            this.horaDeFinalizacion = horaDeFinalizacion;
        }
    }
}



    
