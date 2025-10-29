using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    [Index(nameof(Mail), IsUnique = true)]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string Cedula { get; set; } = string.Empty;

        public string Contra { get; set; } = string.Empty;

        [EmailAddress]
        public string Mail { get; set; } = string.Empty;
        public Usuario() { }

        public Usuario( string nombreCompleto, string cedula, string mail, string contra)
        {
            this.NombreCompleto = nombreCompleto;
            this.Cedula = cedula;
            this.Contra = contra;
            this.Mail = mail;
        }
        public Usuario( string mail, string contra)
        {
            this.Contra = contra;
            this.Mail = mail;
        }
    }
}
