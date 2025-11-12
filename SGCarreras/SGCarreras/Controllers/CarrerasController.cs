using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static SGCarreras.Models.Estado;
using static SGCarreras.Models.Sexo;

namespace SGCarreras.Controllers
{
    public class CarrerasController : Controller
    {
        private readonly SGCarrerasContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public CarrerasController(SGCarrerasContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: Carreras
        public async Task<IActionResult> Index()
        {
          //  await InicializarCarrerasActivasAsync(_clientFactory);

            var carreras = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .ToListAsync();
            return View(carreras);
        }

        // GET: Carreras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carrera
                .Include(m => m.Registros.Where(r => r.confirmado == true))
                .ThenInclude(r => r.Corredor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }

        // GET: Carreras/Create
        public IActionResult Create()
        {
            ViewBag.EstadoList = Enum.GetValues(typeof(EstadoEnum))
                            .Cast<EstadoEnum>()
                            .Select(s => new SelectListItem
                            {
                                Value = s.ToString(),
                                Text = s.ToString()
                            }).ToList();

            return View();
        }

        // POST: Carreras/Create - CON BIND Y DEBUGGING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Ubicacion,Estado,KmTotales,Fecha,PuntosDeControl")] Carrera carrera)
        {
            // DEBUGGING: Ver qué datos llegan
            Console.WriteLine("=== DEBUG CREATE ===");
            Console.WriteLine($"Carrera recibida - Nombre: {carrera.Nombre}, Ubicacion: {carrera.Ubicacion}");
            Console.WriteLine($"PuntosDeControl es null: {carrera.PuntosDeControl == null}");

            if (carrera.PuntosDeControl != null)
            {
                Console.WriteLine($"Cantidad de puntos: {carrera.PuntosDeControl.Count}");
                foreach (var punto in carrera.PuntosDeControl)
                {
                    Console.WriteLine($"Punto - id: {punto.id}, distancia: {punto.Distancia}, CarreraId: {punto.CarreraId}");
                }
            }
            else
            {
                Console.WriteLine("PuntosDeControl es NULL - revisando Request.Form...");

                // Debug del FormCollection
                foreach (var key in Request.Form.Keys)
                {
                    if (key.Contains("PuntosDeControl"))
                    {
                        Console.WriteLine($"Form[{key}] = {Request.Form[key]}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState es válido, guardando carrera...");

                    // 1. Guardar la carrera primero para obtener el ID
                    _context.Add(carrera);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Carrera guardada con ID: {carrera.Id}");

                    // 2. Procesar puntos de control si existen
                    if (carrera.PuntosDeControl != null && carrera.PuntosDeControl.Any())
                    {
                        Console.WriteLine($"Procesando {carrera.PuntosDeControl.Count} puntos de control...");

                        foreach (var punto in carrera.PuntosDeControl)
                        {
                            // Asignar el ID de la carrera al punto de control
                            punto.CarreraId = carrera.Id;
                            _context.PuntosDeControl.Add(punto);
                            Console.WriteLine($"Agregado punto: {punto.Distancia} km para carrera {punto.CarreraId}");
                        }
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Puntos de control guardados exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("No hay puntos de control para guardar");
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    ModelState.AddModelError("", $"Error al crear la carrera: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ModelState NO es válido. Errores:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($" - {error.ErrorMessage}");
                }
            }

            // Recargar ViewBag si hay error
            ViewBag.EstadoList = Enum.GetValues(typeof(EstadoEnum))
                .Cast<EstadoEnum>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                }).ToList();

            return View(carrera);
        }

        // GET: Carreras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }

            ViewBag.EstadoList = Enum.GetValues(typeof(EstadoEnum))
                                    .Cast<EstadoEnum>()
                                    .Select(s => new SelectListItem
                                    {
                                        Value = s.ToString(),
                                        Text = s.ToString(),
                                        Selected = s == carrera.Estado
                                    }).ToList();

            return View(carrera);
        }

        // POST: Carreras/Edit/5 - CON BIND Y DEBUGGING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Ubicacion,Estado,KmTotales,Fecha,PuntosDeControl")] Carrera carrera)
        {
            // DEBUGGING
            Console.WriteLine("=== DEBUG EDIT ===");
            Console.WriteLine($"Editando carrera ID: {id}, Carrera recibida ID: {carrera.Id}");
            Console.WriteLine($"PuntosDeControl es null: {carrera.PuntosDeControl == null}");

            if (carrera.PuntosDeControl != null)
            {
                Console.WriteLine($"Cantidad de puntos recibidos: {carrera.PuntosDeControl.Count}");
                foreach (var punto in carrera.PuntosDeControl)
                {
                    Console.WriteLine($"Punto - id: {punto.id}, distancia: {punto.Distancia}, CarreraId: {punto.CarreraId}");
                }
            }

            if (id != carrera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState válido, procesando edición...");

                    // 1. Eliminar puntos de control existentes
                    var puntosExistentes = _context.PuntosDeControl.Where(p => p.CarreraId == id);
                    var countExistentes = await puntosExistentes.CountAsync();
                    Console.WriteLine($"Eliminando {countExistentes} puntos existentes...");

                    _context.PuntosDeControl.RemoveRange(puntosExistentes);

                    // 2. Actualizar la carrera
                    _context.Update(carrera);
                    await _context.SaveChangesAsync(); // Guardar cambios de la carrera

                    // 3. Agregar nuevos puntos de control
                    if (carrera.PuntosDeControl != null && carrera.PuntosDeControl.Any())
                    {
                        Console.WriteLine($"Agregando {carrera.PuntosDeControl.Count} nuevos puntos...");

                        foreach (var punto in carrera.PuntosDeControl)
                        {
                            punto.CarreraId = carrera.Id;
                            _context.PuntosDeControl.Add(punto);
                            Console.WriteLine($"Agregado punto: {punto.Distancia} km");
                        }
                        await _context.SaveChangesAsync(); // Guardar puntos
                    }

                    Console.WriteLine("Edición completada exitosamente");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarreraExists(carrera.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR en Edit: {ex.Message}");
                    ModelState.AddModelError("", $"Error al editar la carrera: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ModelState NO es válido en Edit. Errores:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($" - {error.ErrorMessage}");
                }
            }

            // Recargar ViewBag si hay error
            ViewBag.EstadoList = Enum.GetValues(typeof(EstadoEnum))
                .Cast<EstadoEnum>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString(),
                    Selected = s == carrera.Estado
                }).ToList();

            return View(carrera);
        }

        // GET: Carreras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }

        // POST: Carreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrera = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrera != null)
            {
                _context.Carrera.Remove(carrera);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarreraExists(int id)
        {
            return _context.Carrera.Any(e => e.Id == id);
        }

        // Listado de Carreras Activas
        public async Task<IActionResult> ListadoActivas()
        {
            var carrerasActivas = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .Where(c => c.Estado == EstadoEnum.Activo)
                .ToListAsync();
            return View(carrerasActivas);
        }

        // Listado de Carreras En Espera
        public async Task<IActionResult> ListadoEnEspera()
        {
            var carrerasEnEspera = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .Where(c => c.Estado == EstadoEnum.En_espera)
                .ToListAsync();
            return View(carrerasEnEspera);
        }

        // Listado de Carreras Finalizadas
        public async Task<IActionResult> ListadoFinalizadas()
        {
            var carrerasFinalizadas = await _context.Carrera
                .Include(c => c.PuntosDeControl)
                .Where(c => c.Estado == EstadoEnum.Finalizada)
                .ToListAsync();
            return View(carrerasFinalizadas);
        }

        public IActionResult Calendarios(string filtro = "Todas")
        {
            IEnumerable<Carrera> carreras;

            switch (filtro)
            {
                case "Activas":
                    carreras = _context.Carrera
                        .Include(c => c.PuntosDeControl)
                        .Where(c => c.Estado == EstadoEnum.Activo).ToList();
                    break;
                case "EnEspera":
                    carreras = _context.Carrera
                        .Include(c => c.PuntosDeControl)
                        .Where(c => c.Estado == EstadoEnum.En_espera).ToList();
                    break;
                case "Finalizadas":
                    carreras = _context.Carrera
                        .Include(c => c.PuntosDeControl)
                        .Where(c => c.Estado == EstadoEnum.Finalizada).ToList();
                    break;
                default:
                    carreras = _context.Carrera
                        .Include(c => c.PuntosDeControl)
                        .ToList();
                    break;
            }

            ViewBag.FiltroActual = filtro;
            return View(carreras);
        }

        public IActionResult CalendariosAdmin(string filtro = "Todas")
        {
            IEnumerable<Carrera> carreras;
            if (corroborarRol("Administrador") == true)
            {
                switch (filtro)
                {
                    case "Activas":
                        carreras = _context.Carrera.Where(c => c.Estado == EstadoEnum.Activo).ToList();
                        break;
                    case "EnEspera":
                        carreras = _context.Carrera.Where(c => c.Estado == EstadoEnum.En_espera).ToList();
                        break;
                    case "Finalizadas":
                        carreras = _context.Carrera.Where(c => c.Estado == EstadoEnum.Finalizada).ToList();
                        break;
                    default:
                        carreras = _context.Carrera.ToList();
                        break;
                }

                ViewBag.FiltroActual = filtro;
                return View("AdminShenanigans/CalendariosAdmin", carreras);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
     
        }
        public async Task<IActionResult> Registros(int? id)
        {
            var carrera = await _context.Carrera
                .Include(m => m.Registros.Where(r => r.confirmado == false))
                .ThenInclude(r => r.Corredor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }


            return View("AdminShenanigans/ListadoRegistro", carrera.Registros);
        }
        private bool corroborarRol(string rol)
        {
            var rolToken = User.FindFirstValue(ClaimTypes.Role);
            if (rolToken != null && rolToken == rol)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public async Task InicializarCarrerasActivasAsync(IHttpClientFactory clientFactory)
        {
            var client = clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7247/api/Simulacion/importar"); // URL de la API

            var carrera = await _context.Carrera
            .Include(c => c.Registros.Where(r => r.confirmado == true))
                .ThenInclude(r => r.Corredor)
            .Include(c => c.PuntosDeControl) 
            .ToListAsync();

            var activas = carrera
                .Where(c => c.Estado == EstadoEnum.Activo)
                .ToList();

            // 🔍 Serializamos para ver qué se está enviando
            var json = JsonSerializer.Serialize(activas, new JsonSerializerOptions
            {
                WriteIndented = true // lo hace más legible
            });

            Console.WriteLine("===------------------------------------ JSON a enviar a la API ===-------------------------------------");
            Console.WriteLine(json);
            Console.WriteLine("===------------------------------------ JSON a enviar a la API ===-------------------------------------");
            // 🚀 Enviar el JSON a la API
            await client.PostAsJsonAsync("/api/Simulacion/importar", activas);
        }

    }
}