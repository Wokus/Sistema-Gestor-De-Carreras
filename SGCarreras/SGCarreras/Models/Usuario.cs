using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    [Index(nameof(mail), IsUnique = true)]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string nombreCompleto { get; set; }

        public string cedula { get; set; }

        public string contra { get; set; }

        [EmailAddress]
        public string mail { get; set; }
        public Usuario() { }

        public Usuario( string nombreCompleto, string cedula, string mail, string contra)
        {
            this.nombreCompleto = nombreCompleto;
            this.cedula = cedula;
            this.contra = contra;
            this.mail = mail;
        }
        public Usuario( string mail, string contra)
        {
           
            this.contra = contra;
            this.mail = mail;
        }
    }
}



    
