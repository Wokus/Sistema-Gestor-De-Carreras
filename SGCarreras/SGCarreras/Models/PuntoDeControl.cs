namespace SGCarreras.Models
{
    public class PuntoDeControl
    {
        public int  nmro { get; set; }

        public double distancia { get; set; }

        //Relacioneison
        public Carrera Carrera { get; set; }
        public PuntoDeControl() { }

        public PuntoDeControl(int nmro, double distancia)
        {
            this.nmro = nmro;
            this.distancia = distancia;
        }
    }
}



    
