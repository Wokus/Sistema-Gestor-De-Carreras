using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SGCarreras.Models.ViewModels
{
    /*
                                  carreraId = carrera.Id,
                                  carreraNombre = carrera.Nombre,
                                  corredorId = registro.Corredor.Id,
                                  corredorNombre = registro.Corredor.NombreCompleto,
                                  posicionCarrera = registro.PosicionEnCarrera,
                                  tiempo = registro.HoraAvance,
                                  kilometro = kmtrsPunto
  */

    public class CorredorActivo
    {
        //de corredor/registro
        public string corredorNombre { get; set; } = string.Empty;
        public int nmroEnCarrera { get; set; }
        public int corredorId { get; set; }
        public int registroId { get; set; }

        //de carrera
        public string carreraNombre { get; set; }
        public int carreraId { get; set; }

        //de sumulacion
        public int posicionCarrera { get; set; }
        public DateTime tiempo { get; set; }
        public int kilometro { get; set; }

        public double kmTotalesCarrera { get; set; }
        public List<PuntoDeControl> puntosDeControl { get; set; } = new List<PuntoDeControl>();


        public CorredorActivo() { }

    }
     
}
