using static SGCarreras.Models.Sexo;

namespace SGCarreras.Models.dtos;

public class CorredorDTO
{

    public int Id { get; set; }
    public string NombreCompleto { get; set; }

    public string Mail { get; set; }
    public string Cedula { get; set; }
    public SexoEnum Sexo { get; set; }
    public string Nacionalidad { get; set; } = string.Empty;

    public ICollection<Inscripcion> Registros { get; set; } = [];

    public CorredorDTO()
    {

    }
}
