using SGCarreras.Models;
using System.ComponentModel.DataAnnotations;
using static SGCarreras.Models.Sexo;

namespace SGCarreras.Models.ViewModels
{
    // ViewModel para el formulario de inscripción
    public class InscripcionViewModel
    {
        public int CarreraId { get; set; }
        public bool EsUsuarioRegistrado { get; set; }

        // Datos para usuarios no registrados
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        public string? Cedula { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string? Contra { get; set; }

        [Required(ErrorMessage = "Confirmar contraseña es obligatorio")]
        [Compare("Contra", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarContra { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio")]
        public SexoEnum? Sexo { get; set; }

        [Required(ErrorMessage = "La nacionalidad es obligatoria")]
        public string? Nacionalidad { get; set; }

        // Para usuarios registrados
        public Corredor? CorredorExistente { get; set; }
    }
}
