using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCarreras.Models
{
    public enum EstadoInscripcion
    {
        Pendiente,
        Confirmada,
        Rechazada,
        Cancelada
    }

    public class Inscripcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

        [Required]
        public int NumeroCorredor { get; set; }

        [Required]
        public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Pendiente;

        // Relaciones
        [Required]
        public int CorredorId { get; set; }

        [Required]
        public int CarreraId { get; set; }


        // Navigation properties
        [ForeignKey("CorredorId")]
        public Corredor? Corredor { get; set; }

        [ForeignKey("CarreraId")]
        public Carrera? Carrera { get; set; }

        // Relación 1:1 con Registro (cuando el corredor completa la carrera)
        public Registro? Registro { get; set; }

        public Inscripcion() { }

        public Inscripcion(int corredorId, int carreraId, int numeroCorredor)
        {
            CorredorId = corredorId;
            CarreraId = carreraId;
            NumeroCorredor = numeroCorredor;
            FechaInscripcion = DateTime.UtcNow;
        }
    }
}
