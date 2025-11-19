// SGCarreras.Models.ViewModels/CarreraDetailsViewModel.cs
using System.Collections.Generic;
using SGCarreras.Models;

namespace SGCarreras.Models.ViewModels
{
    public class CarreraDetailsViewModel
    {
        public Carrera Carrera { get; set; }
        public List<Registro> Registros { get; set; } = new List<Registro>();
        public bool YaInscrito { get; set; }
        // Puedes añadir más propiedades según lo necesites en la vista
    }
}