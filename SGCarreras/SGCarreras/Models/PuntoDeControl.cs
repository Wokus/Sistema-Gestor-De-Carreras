namespace SGCarreras.Models
{
    public class PuntoDeControl
    {
        public int  id { get; set; }

        public double distancia { get; set; }

        //Relacioneison
        public Carrera Carrera { get; set; }
        public PuntoDeControl() { }

        public PuntoDeControl(int id, double distancia)
        {
            this.id = id;
            this.distancia = distancia;
        }
    }
}



    
