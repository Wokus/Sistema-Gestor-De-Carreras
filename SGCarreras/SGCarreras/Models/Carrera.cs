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

        // Relaciones

        public ICollection<Registro> Registros { get; set; } = [];

        public ICollection<PuntoDeControl> PuntosDeControl { get; set; } = [];

        public Carrera() { }

        public Carrera(string nombre, string ubicacion, EstadoEnum estado, double kmTotales, DateTime fecha)
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
            Estado = estado;
            KmTotales = kmTotales;
            Registros = [];
            PuntosDeControl = [];
            Fecha = fecha;
        }
    }
}
