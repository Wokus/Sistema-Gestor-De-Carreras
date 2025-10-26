using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SGCarreras.Models.ViewModels
{
    [Index(nameof(mail), IsUnique = true)]
    public class CorredorLogIn
    {

        [EmailAddress]
        public string mail { get; set; }
        public string contra { get; set; }

      
    }
}
