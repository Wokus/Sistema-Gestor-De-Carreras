using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models.apiThings;
using SGCarreras.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;

namespace apiCarreras.Controllers
{
    [ApiController]
    [Route("apiController/Simulacion")]
    public class apiController : ControllerBase
    {
        private readonly SGCarrerasContext _context;
        private readonly ILogger<apiController> _logger;
        public apiController(ILogger<apiController> logger, SGCarrerasContext context)
        {
            _context = context;

            _logger = logger;
        }
        

        // Cachear JsonSerializerOptions para mejor rendimiento
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        [HttpPost("terminarCarrera")]
        public async Task<IActionResult> ImportarCarreras([FromBody] List<GanaDoorDTO> ganadores)
        {
            if (ganadores == null || ganadores.Count == 0)
                return BadRequest("No se recibieron ganadores");

            
            string json = JsonSerializer.Serialize(ganadores, _jsonSerializerOptions);

            Console.WriteLine("=== JSON RECIBIDO ===");
            Console.WriteLine(json);
            Console.WriteLine("======================");

            int tokenId = ganadores.First().inscripcionId;
            List<GanaDoorDTO> lista = new List<GanaDoorDTO>();
            foreach (var ganador in ganadores)
            {
                try
                {
                    var inscri = await _context.Inscripcion
                .Include(i => i.Carrera)
                .Include(i => i.Corredor)
                .FirstOrDefaultAsync(i => i.Id == ganador.inscripcionId);

                    if (inscri == null)
                    {
                        return NotFound();
                    }
                 
                    Registro regi = new Registro();

                    regi.CorredorId = inscri.CorredorId;
                    regi.CarreraId = inscri.CarreraId;
                    regi.HoraDeFinalizacion = DateTime.ParseExact(ganador.tiempoDeFinalizacion, "HH:mm:ss", null);
                    regi.NumeroEnCarrera = inscri.NumeroCorredor;
                    regi.PosicionEnCarrera = ganador.numeroEnCarrera;
                    _context.Add(regi);
                    inscri.Carrera.Estado = Estado.EstadoEnum.Finalizada;
                    await _context.SaveChangesAsync();


                }
                catch (Exception ex)
                {
                   Console.WriteLine(ex.StackTrace);
                    return StatusCode(500, new
                    {
                        mensaje = "Ocurrió un error inesperado durante la importación de ganadores",
                        error = ex.Message
                    });
                }
            }

            return Ok(new
            {
                mensaje = "gandores obtenidos y carrera detenida",
                //cantidad = carreras.Count,
               // carrerasRecibidas = carreras
            });
        }

        
    }
}
