namespace SGCarreras.Models
{
    public class TiempoParcial
    {
        public int tiempo { get; set; }


        //Relaciones Tiempo de:

        public PuntoDeControl PuntoControl { get; set; }
        public Registro Registro { get; set; }

        public TiempoParcial() { }

        public TiempoParcial(int tiempo)
        {
            this.tiempo = tiempo;
        }
    }
}



    
