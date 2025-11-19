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
using SGCarreras.Models.ViewModels;
using static SGCarreras.Models.Estado;
using static SGCarreras.Models.Sexo;

namespace SGCarreras.Controllers
{
    public class CarrerasController(SGCarrerasContext context, IHttpClientFactory clientFactory) : Controller
    {
        private readonly SGCarrerasContext _context = context;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

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
                .Include(c => c.PuntosDeControl)
                .Include(c => c.Inscripciones.Where(i => i.Estado == EstadoInscripcion.Confirmada))
                    .ThenInclude(i => i.Corredor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (carrera == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro
                .Where(r => r.CarreraId == carrera.Id)
                .Include(r => r.Corredor)
                .OrderBy(r => r.PosicionEnCarrera)
                .ToListAsync();

            bool yaInscrito = false;
            if (User.Identity?.IsAuthenticated == true)
            {
                var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(usuarioIdString, out int usuarioId))
                {
                    yaInscrito = await _context.Inscripcion
                        .AnyAsync(i => i.CorredorId == usuarioId &&
                                      i.CarreraId == id.Value &&
                                      i.Estado != EstadoInscripcion.Cancelada);
                }
            }

            var viewModel = new CarreraDetailsViewModel
            {
                Carrera = carrera,
                Registros = registros,
                YaInscrito = yaInscrito
            };

            return View(viewModel);
        }

        public async Task<IActionResult> SeguimientoDeCorredor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera2 = await _context.Inscripcion
                .Include(c => c.Carrera)
                .ThenInclude(c => c.PuntosDeControl).Where(i => i.Estado == EstadoInscripcion.Confirmada && i.Id == id)
                .FirstOrDefaultAsync();

            var carrera = await _context.Carrera
               .Include(c => c.Inscripciones.Where(i => i.Estado == EstadoInscripcion.Confirmada && i.Id == id))
               .Include(c => c.PuntosDeControl)
               .FirstOrDefaultAsync(c => c.Id == carrera2.Carrera.Id);



            if (carrera?.Inscripciones?.Count > 0)
            {
                await _context.Entry(carrera.Inscripciones.First())
                    .Reference(i => i.Registro)
                    .Query()
                    .Include(r => r.Corredor)
                    .LoadAsync();

                await _context.Entry(carrera.Inscripciones.First())
                    .Reference(i => i.Corredor)
                    .LoadAsync();
            }

            var registro = await _context.Inscripcion
                .Include(r => r.Corredor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro == null || registro.Corredor == null)
            {
                return NotFound();
            }

            // Verificar que carrera no sea null antes de usarla
            if (carrera == null)
            {
                return NotFound();
            }

            CorredorActivo correAct = new()
            {
                nmroEnCarrera = registro.NumeroCorredor,
                corredorNombre = registro.Corredor.NombreCompleto ?? "Nombre no disponible",
                corredorId = registro.Corredor.Id,
                carreraId = carrera.Id,
                carreraNombre = carrera.Nombre ?? "Carrera sin nombre",
                registroId = registro.Id,
                kilometro = 0, // ✅ AGREGAR kilometro inicial
                kmTotalesCarrera = carrera.KmTotales, // ✅ AGREGAR kilómetros totales
                puntosDeControl = carrera.PuntosDeControl?.OrderBy(p => p.Distancia).ToList() ?? new List<PuntoDeControl>() // ✅ AGREGAR puntos de control ordenados
            };

            return View(correAct);
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

        public IActionResult BuscarCorredorCorriendoce()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuscarCorredorCorriendoce([Bind("Cedula")] Corredor2 corre)
        {
            if (corre == null || string.IsNullOrEmpty(corre.Cedula))
            {
                return NotFound();
            }

            Console.WriteLine("&/&/&/&/&/&/&/&/&/&/&/&/&/&/&" + corre.Cedula + "&/&/&/&/&/&/&/&/&/&/&/&/&/&/&");

            var inscri = await _context.Inscripcion
               .Include(i => i.Carrera)
               .Include(i => i.Corredor)
               .Where(i => i.Estado == EstadoInscripcion.Confirmada
                           && i.Corredor != null
                           && i.Corredor.Cedula == corre.Cedula
                           && i.Carrera != null
                           && i.Carrera.Estado == EstadoEnum.Activo)
               .FirstOrDefaultAsync();

            if (inscri == null)
            {
                return NotFound();
            }

            return RedirectToAction("SeguimientoDeCorredor", new { id = inscri.Id });
        }

        // POST: Carreras/Create - CON BIND Y DEBUGGING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Ubicacion,Estado,KmTotales,Fecha,PuntosDeControl")] Carrera carrera)
        {
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

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    Console.WriteLine("ModelState es válido, guardando carrera...");

                    // ✅ SOLUCIÓN: Guardar la carrera CON los puntos de control de una vez
                    // Entity Framework manejará automáticamente las relaciones
                    _context.Add(carrera);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Carrera guardada con ID: {carrera.Id}");
                    Console.WriteLine($"Puntos de control guardados automáticamente: {carrera.PuntosDeControl?.Count ?? 0}");

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Ubicacion,Estado,KmTotales,Fecha,PuntosDeControl")] Carrera carrera)
        {
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
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    Console.WriteLine("ModelState válido, procesando edición...");

                    // 1. Cargar la carrera existente con sus puntos de control
                    var carreraExistente = await _context.Carrera
                        .Include(c => c.PuntosDeControl)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (carreraExistente == null)
                    {
                        return NotFound();
                    }

                    // 2. Actualizar propiedades básicas de la carrera
                    carreraExistente.Nombre = carrera.Nombre;
                    carreraExistente.Ubicacion = carrera.Ubicacion;
                    carreraExistente.Estado = carrera.Estado;
                    carreraExistente.KmTotales = carrera.KmTotales;
                    carreraExistente.Fecha = carrera.Fecha;

                    // 3. Manejar puntos de control
                    // Limpiar puntos existentes
                    _context.PuntosDeControl.RemoveRange(carreraExistente.PuntosDeControl);

                    // Agregar nuevos puntos (si los hay)
                    if (carrera.PuntosDeControl != null && carrera.PuntosDeControl.Any())
                    {
                        foreach (var punto in carrera.PuntosDeControl)
                        {
                            // Crear NUEVAS instancias para evitar problemas de tracking
                            var nuevoPunto = new PuntoDeControl
                            {
                                CarreraId = carreraExistente.Id,
                                Distancia = punto.Distancia
                            };
                            _context.PuntosDeControl.Add(nuevoPunto);
                        }
                    }

                    // 4. Guardar todos los cambios de una vez
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Edición completada exitosamente");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
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
                    await transaction.RollbackAsync();
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
            IEnumerable<Carrera> carreras = filtro switch
            {
                "Activas" => [.. _context.Carrera
                                        .Include(c => c.PuntosDeControl)
                                        .Where(c => c.Estado == EstadoEnum.Activo)],
                "EnEspera" => [.. _context.Carrera
                                        .Include(c => c.PuntosDeControl)
                                        .Where(c => c.Estado == EstadoEnum.En_espera)],
                "Finalizadas" => [.. _context.Carrera
                                        .Include(c => c.PuntosDeControl)
                                        .Where(c => c.Estado == EstadoEnum.Finalizada)],
                _ => _context.Carrera
                                        .Include(c => c.PuntosDeControl)
                                        .ToList(),
            };
            ViewBag.FiltroActual = filtro;
            return View(carreras);
        }

        public IActionResult CalendariosAdmin(string filtro = "Todas")
        {
            IEnumerable<Carrera> carreras;
            if (CorroborarRol("Administrador") == true)
            {
                carreras = filtro switch
                {
                    "Activas" => [.. _context.Carrera.Where(c => c.Estado == EstadoEnum.Activo)],
                    "EnEspera" => [.. _context.Carrera.Where(c => c.Estado == EstadoEnum.En_espera)],
                    "Finalizadas" => [.. _context.Carrera.Where(c => c.Estado == EstadoEnum.Finalizada)],
                    _ => _context.Carrera.ToList(),
                };
                ViewBag.FiltroActual = filtro;
                return View("AdminShenanigans/CalendariosAdmin", carreras);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
     
        }

        private bool CorroborarRol(string rol)
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

        public JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true // lo hace más legible
            };
        }

        public async Task InicializarCarrerasActivasAsync(IHttpClientFactory clientFactory, JsonSerializerOptions options)
        {
            var client = clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7247/api/Simulacion/importar"); // URL de la API

            var carrera = await _context.Carrera
            .Include(c => c.Inscripciones.Where(i => i.Estado == EstadoInscripcion.Confirmada))
            .ThenInclude(i => i!.Registro)
            .ThenInclude(r => r!.Corredor)
            .Include(c => c.Inscripciones.Where(i => i.Estado == EstadoInscripcion.Confirmada))
            .ThenInclude(i => i.Corredor)
            .Include(c => c.PuntosDeControl)
            .ToListAsync();


            var activas = carrera
                .Where(c => c.Estado == EstadoEnum.Activo)
                .ToList();

            // 🔍 Serializamos para ver qué se está enviando
            var json = JsonSerializer.Serialize(activas, options);

            Console.WriteLine("===------------------------------------ JSON a enviar a la API ===-------------------------------------");
            Console.WriteLine(json);
            Console.WriteLine("===------------------------------------ JSON a enviar a la API ===-------------------------------------");

            await client.PostAsJsonAsync("/api/Simulacion/importar", activas);
        }

    }
}