using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SGCarreras.Models.ViewModels
{
    [Index(nameof(Mail), IsUnique = true)]
    public class CorredorLogIn
    {

        [EmailAddress]
        public string Mail { get; set; } = string.Empty;
        public string Contra { get; set; } = string.Empty;

    }
}
