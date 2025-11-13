using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SGCarreras.Models.Estado;

namespace SGCarreras.Models
{
    public class Carrera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [StringLength(150, ErrorMessage = "La ubicación no puede exceder los 150 caracteres.")]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public EstadoEnum Estado { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Los kilómetros totales deben ser un valor positivo.")]
        public double KmTotales { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha y Hora")]
        public DateTime Fecha { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El máximo de participantes debe ser un valor positivo.")]
        [Display(Name = "Máximo de participantes")]

        // RELACIONES ACTUALIZADAS
        public ICollection<Inscripcion> Inscripciones { get; set; } = [];

        public ICollection<PuntoDeControl> PuntosDeControl { get; set; } = [];

        // La relación directa con Registros se elimina, ya que ahora 
        // los Registros se acceden a través de Inscripciones → Registro

        public Carrera() { }

        public Carrera(string nombre, string ubicacion, EstadoEnum estado, double kmTotales, DateTime fecha)
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
            Estado = estado;
            KmTotales = kmTotales;
            Fecha = fecha;
            Inscripciones = [];
            PuntosDeControl = [];
        }

        // PROPIEDADES CALCULADAS ÚTILES
        [NotMapped]
        [Display(Name = "Total de inscritos")]
        public int TotalInscritosConfirmados =>
            Inscripciones?.Count(i => i.Estado == EstadoInscripcion.Confirmada) ?? 0;

    }
}
