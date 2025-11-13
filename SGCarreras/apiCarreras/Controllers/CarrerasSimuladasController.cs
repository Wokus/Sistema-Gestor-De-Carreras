using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using apiCarreras.Hubs;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using apiCarreras.DTOs;
using System.Text.Json;
using apiCarreras.Services;

namespace apiCarreras.Controllers
{
    [ApiController]
    [Route("api/Simulacion")]
    public class CarrerasSimuladasController : ControllerBase
    {
        private readonly IHubContext<CarrerasSimuladasHub> _hubContext;
        private readonly ISimuladorService _simulador;
        private readonly ILogger<CarrerasSimuladasController> _logger;
        private static Dictionary<int, bool> _simulacionesActivas = new();
        public CarrerasSimuladasController(IHubContext<CarrerasSimuladasHub> hubContext, ISimuladorService simulador, ILogger<CarrerasSimuladasController> logger)
        {
            _hubContext = hubContext;
            _simulador = simulador;
            _logger = logger;
        }

        [HttpPost("importar")]
        public IActionResult ImportarCarreras([FromBody] List<CarreraDTO> carreras)
        {

            if (carreras == null || carreras.Count == 0)
                return BadRequest("No se recibieron carreras");

            var json = JsonSerializer.Serialize(carreras, new JsonSerializerOptions
            {
                WriteIndented = true 
            });

            Console.WriteLine("=== JSON RECIBIDO ===");
            Console.WriteLine(json);
            Console.WriteLine("======================");
            
            foreach (var carrera in carreras)
            {
               
                try
                {
                    _simulador.IniciarSimulacion(carrera);
                    Console.WriteLine($" Simulación iniciada para carrera: {carrera.Nombre ?? "(sin nombre)"}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error al iniciar simulación para la carrera {carrera.Nombre ?? "(sin nombre)"}: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                     return StatusCode(500, new
        {
            mensaje = "Ocurrió un error inesperado durante la importación de carreras",
            error = ex.Message
        });
                }
            }

            return Ok(new
            {
                mensaje = "Simulaciones iniciadas",
                cantidad = carreras.Count,
                carrerasRecibidas = carreras
            });
           
        }

        /*[HttpPost("iniciar")]
        public async Task<IActionResult> IniciarCarrera([FromBody] List<CarreraDTO>  carreras)
        {
            if(carreras == null)
                return BadRequest("Datos inválidos");

            


            foreach (var carrera in carreras)
            {
                if (!_simulacionesActivas.ContainsKey(carrera.Id))
                {
                    _simulacionesActivas[carrera.Id] = true;

                    _ = Task.Run(async () =>
                    {
                        var rnd = new Random();

                        
                    });



                }
                else
                {
                    return BadRequest("La simulación ya está en curso.");
                }




                   



            }
            

            return Ok($"Carrera {carrera.Nombre} iniciada.");
        }

        [HttpPost("IniciarMultiples")]
        public IActionResult IniciarMultiples([FromBody] List<int> carrerasIds)
        {
            foreach (var id in carrerasIds)
            {
                if (!_simulacionesActivas.ContainsKey(id))
                {
                    _ = Task.Run(() => SimularCarrera(id));
                }
            }
            return Ok(new { mensaje = $"{carrerasIds.Count} carreras inicializadas." });
        }


        [HttpPost("detener/{id}")]
        public IActionResult DetenerCarrera(int id)
        {
            _simulacionesActivas[id] = false;
            return Ok("Carrera detenida.");
        }
       */
     }

}
