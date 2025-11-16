using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using System.Diagnostics;
using System.Text.Json;
using static SGCarreras.Models.Estado;


namespace SGCarreras.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SGCarrerasContext _context;
        public HomeController(ILogger<HomeController> logger, SGCarrerasContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task InicializarCarrerasActivasAsync(IHttpClientFactory clientFactory)
        {
            var client = clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7247"); // URL de la API

            var carrera = await _context.Carrera
                .Include(m => m.Registros.Where(r => r.confirmado == false))
                .ThenInclude(r => r.Corredor)
                .ToListAsync();

            var activas = carrera
                .Where(c => c.Estado == EstadoEnum.Activo)
                .ToList();

            // 🔍 Serializamos para ver qué se está enviando
            var json = JsonSerializer.Serialize(activas, new JsonSerializerOptions
            {
                WriteIndented = true // lo hace más legible
            });

            Console.WriteLine("=== JSON a enviar a la API ===");
            Console.WriteLine(json);

            // 🚀 Enviar el JSON a la API
            _logger.LogInformation("Enviando carreras activas: {json}", json);
            await client.PostAsJsonAsync("/api/CarrerasSimuladasController/importar", activas);
        }

    }
}
