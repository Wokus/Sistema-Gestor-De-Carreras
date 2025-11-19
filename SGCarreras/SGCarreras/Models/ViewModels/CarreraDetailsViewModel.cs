// SGCarreras.Models.ViewModels/CarreraDetailsViewModel.cs
using System.Collections.Generic;
using SGCarreras.Models;

namespace SGCarreras.Models.ViewModels
{
    public class CarreraDetailsViewModel
    {
        public required Carrera Carrera { get; set; }
        public List<Registro> Registros { get; set; } = [];
        public bool YaInscrito { get; set; }
    }
}
