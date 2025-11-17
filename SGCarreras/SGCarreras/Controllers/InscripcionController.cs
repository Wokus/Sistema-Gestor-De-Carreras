using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using SGCarreras.Models.ViewModels;
using System.Security.Claims;

namespace SGCarreras.Controllers
{
    public class InscripcionController : Controller
    {
        private readonly SGCarrerasContext _context;

        public InscripcionController(SGCarrerasContext context)
        {
            _context = context;
        }

        // GET: Inscripcion
        public async Task<IActionResult> Index()
        {
            var inscripciones = await _context.Inscripcion
                .Include(i => i.Corredor)
                .Include(i => i.Carrera)
                .Include(i => i.Registro)
                .ToListAsync();

            return View(inscripciones);
        }

        // GET: Inscripcion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcion
                .Include(i => i.Corredor)
                .Include(i => i.Carrera)
                .Include(i => i.Registro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // GET: Inscripcion/Create
        public IActionResult Create()
        {
            ViewData["CorredorId"] = new SelectList(_context.Corredor, "Id", "NombreCompleto");
            ViewData["CarreraId"] = new SelectList(_context.Carrera.Where(c => c.Estado == Estado.EstadoEnum.Activo || c.Estado == Estado.EstadoEnum.En_espera), "Id", "Nombre");
            return View();
        }

        // POST: Inscripcion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CorredorId,CarreraId,NumeroCorredor,Observaciones")] Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe una inscripción para este corredor en esta carrera
                var existeInscripcion = await _context.Inscripcion
                    .AnyAsync(i => i.CorredorId == inscripcion.CorredorId &&
                                  i.CarreraId == inscripcion.CarreraId &&
                                  i.Estado != EstadoInscripcion.Cancelada);

                if (existeInscripcion)
                {
                    ModelState.AddModelError(string.Empty, "El corredor ya está inscrito en esta carrera.");
                    ViewData["CorredorId"] = new SelectList(_context.Corredor, "Id", "NombreCompleto", inscripcion.CorredorId);
                    ViewData["CarreraId"] = new SelectList(_context.Carrera.Where(c => c.Estado == Estado.EstadoEnum.Activo || c.Estado == Estado.EstadoEnum.En_espera), "Id", "Nombre", inscripcion.CarreraId);
                    return View(inscripcion);
                }

                // Asignar estado inicial
                inscripcion.Estado = EstadoInscripcion.Pendiente;
                inscripcion.FechaInscripcion = DateTime.UtcNow;

                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CorredorId"] = new SelectList(_context.Corredor, "Id", "NombreCompleto", inscripcion.CorredorId);
            ViewData["CarreraId"] = new SelectList(_context.Carrera.Where(c => c.Estado == Estado.EstadoEnum.Activo || c.Estado == Estado.EstadoEnum.En_espera), "Id", "Nombre", inscripcion.CarreraId);
            return View(inscripcion);
        }

        // GET: Inscripcion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcion.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            ViewData["CorredorId"] = new SelectList(_context.Corredor, "Id", "NombreCompleto", inscripcion.CorredorId);
            ViewData["CarreraId"] = new SelectList(_context.Carrera, "Id", "Nombre", inscripcion.CarreraId);
            ViewData["EstadoInscripcion"] = new SelectList(Enum.GetValues(typeof(EstadoInscripcion)), inscripcion.Estado);
            return View(inscripcion);
        }

        // POST: Inscripcion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CorredorId,CarreraId,NumeroCorredor,Estado,Observaciones,FechaInscripcion")] Inscripcion inscripcion)
        {
            if (id != inscripcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcionExists(inscripcion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CorredorId"] = new SelectList(_context.Corredor, "Id", "NombreCompleto", inscripcion.CorredorId);
            ViewData["CarreraId"] = new SelectList(_context.Carrera, "Id", "Nombre", inscripcion.CarreraId);
            ViewData["EstadoInscripcion"] = new SelectList(Enum.GetValues(typeof(EstadoInscripcion)), inscripcion.Estado);
            return View(inscripcion);
        }

        // GET: Inscripcion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcion
                .Include(i => i.Corredor)
                .Include(i => i.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // POST: Inscripcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcion = await _context.Inscripcion.FindAsync(id);
            if (inscripcion != null)
            {
                _context.Inscripcion.Remove(inscripcion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Inscripcion/CambiarEstado/5
        public async Task<IActionResult> CambiarEstado(int id, EstadoInscripcion nuevoEstado)
        {
            var inscripcion = await _context.Inscripcion.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            inscripcion.Estado = nuevoEstado;
            _context.Update(inscripcion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Inscripcion/PorCarrera/5
        public async Task<IActionResult> PorCarrera(int carreraId)
        {
            var carrera = await _context.Carrera
                .Include(c => c.Inscripciones)
                    .ThenInclude(i => i.Corredor)
                .Include(c => c.Inscripciones)
                    .ThenInclude(i => i.Registro)
                .FirstOrDefaultAsync(c => c.Id == carreraId);

            if (carrera == null)
            {
                return NotFound();
            }

            ViewData["CarreraNombre"] = carrera.Nombre;
            return View(carrera.Inscripciones.ToList());
        }

        // GET: Inscripcion/PorCorredor/5
        public async Task<IActionResult> PorCorredor(int corredorId)
        {
            var corredor = await _context.Corredor
                .Include(c => c.registros)
                .FirstOrDefaultAsync(c => c.Id == corredorId);

            if (corredor == null)
            {
                return NotFound();
            }

            var inscripciones = await _context.Inscripcion
                .Where(i => i.CorredorId == corredorId)
                .Include(i => i.Carrera)
                .Include(i => i.Registro)
                .ToListAsync();

            ViewData["CorredorNombre"] = corredor.NombreCompleto;
            return View(inscripciones);
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripcion.Any(e => e.Id == id);
        }

        // GET: Inscripcion/Inscribirse/5
        public async Task<IActionResult> Inscribirse(int carreraId)
        {
            Console.WriteLine($"GET Inscribirse - CarreraId: {carreraId}");

            var carrera = await _context.Carrera
                .FirstOrDefaultAsync(c => c.Id == carreraId &&
                                         (c.Estado == Estado.EstadoEnum.En_espera || c.Estado == Estado.EstadoEnum.Activo));

            if (carrera == null)
            {
                Console.WriteLine("Carrera no encontrada o no disponible");
                TempData["Error"] = "Carrera no encontrada o no disponible para inscripción.";
                return RedirectToAction("Calendarios", "Carreras");
            }

            ViewData["Carrera"] = carrera;

            // Verificar si el usuario está autenticado
            if (User.Identity.IsAuthenticated)
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Verificar si es administrador
                var esAdministrador = User.IsInRole("Administrador");
                if (esAdministrador)
                {
                    TempData["Error"] = "Los administradores no pueden inscribirse en carreras.";
                    return RedirectToAction("Calendarios", "Carreras");
                }

                // Verificar si ya está inscrito
                var yaInscrito = await _context.Inscripcion
                    .AnyAsync(i => i.CorredorId == usuarioId &&
                                  i.CarreraId == carreraId &&
                                  i.Estado != EstadoInscripcion.Cancelada);

                if (yaInscrito)
                {
                    TempData["Error"] = "Ya estás inscrito en esta carrera.";
                    return RedirectToAction("Calendarios", "Carreras");
                }

                // Usuario logueado - mostrar formulario simple de confirmación
                var corredor = await _context.Corredor.FindAsync(usuarioId);
                var viewModel = new InscripcionViewModel
                {
                    CarreraId = carreraId,
                    EsUsuarioRegistrado = true,
                    CorredorExistente = corredor
                };

                return View(viewModel);
            }
            else
            {
                // Usuario no logueado - mostrar formulario completo de registro + inscripción
                var viewModel = new InscripcionViewModel
                {
                    CarreraId = carreraId,
                    EsUsuarioRegistrado = false
                };

                return View(viewModel);
            }
        }

        // POST: Inscripcion/Inscribirse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribirse(InscripcionViewModel model)
        {
            Console.WriteLine($"POST Inscribirse - CarreraId: {model.CarreraId}, EsUsuarioRegistrado: {model.EsUsuarioRegistrado}");

            var carrera = await _context.Carrera.FindAsync(model.CarreraId);
            if (carrera == null)
            {
                TempData["Error"] = "Carrera no encontrada.";
                return RedirectToAction("Calendarios", "Carreras");
            }

            // SOLUCIÓN: Si es usuario registrado, limpiar los errores de campos que no usa
            if (model.EsUsuarioRegistrado)
            {
                ModelState.Remove("NombreCompleto");
                ModelState.Remove("Cedula");
                ModelState.Remove("Email");
                ModelState.Remove("Contra");
                ModelState.Remove("ConfirmarContra");
                ModelState.Remove("Sexo");
                ModelState.Remove("Nacionalidad");
            }

            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState no es válido");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine("Procesando inscripción...");
                if (model.EsUsuarioRegistrado)
                {
                    return await InscribirUsuarioExistente(model);
                }
                else
                {
                    return await RegistrarEInscribirNuevoUsuario(model);
                }
            }

            ViewData["Carrera"] = carrera;
            Console.WriteLine("Retornando vista con errores");
            return View(model);
        }

        private async Task<IActionResult> InscribirUsuarioExistente(InscripcionViewModel model)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verificar si ya está inscrito (doble verificación)
            var yaInscrito = await _context.Inscripcion
                .AnyAsync(i => i.CorredorId == usuarioId &&
                              i.CarreraId == model.CarreraId &&
                              i.Estado != EstadoInscripcion.Cancelada);

            if (yaInscrito)
            {
                TempData["Error"] = "Ya estás inscrito en esta carrera.";
                return RedirectToAction("Calendarios", "Carreras");
            }

            // Crear inscripción
            var numeroCorredor = await GenerarNumeroCorredor(model.CarreraId);
            var inscripcion = new Inscripcion
            {
                CorredorId = usuarioId,
                CarreraId = model.CarreraId,
                NumeroCorredor = numeroCorredor,
                Estado = EstadoInscripcion.Pendiente,
                FechaInscripcion = DateTime.UtcNow,
            };

            _context.Inscripcion.Add(inscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "¡Inscripción realizada con éxito!";
            return RedirectToAction("Details", "Carreras", new { id = model.CarreraId });
        }

        private async Task<IActionResult> RegistrarEInscribirNuevoUsuario(InscripcionViewModel model)
        {
            // Verificar si el email ya existe
            var emailExiste = await _context.Corredor.AnyAsync(u => u.Mail == model.Email);
            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Ya existe un usuario con este email.");
                ViewData["Carrera"] = await _context.Carrera.FindAsync(model.CarreraId);
                return View(model);
            }

            // Verificar si la cédula ya existe
            var cedulaExiste = await _context.Corredor.AnyAsync(u => u.Cedula == model.Cedula);
            if (cedulaExiste)
            {
                ModelState.AddModelError("Cedula", "Ya existe un usuario con esta cédula.");
                ViewData["Carrera"] = await _context.Carrera.FindAsync(model.CarreraId);
                return View(model);
            }

            // Crear nuevo corredor
            var corredor = new Corredor
            {
                NombreCompleto = model.NombreCompleto!,
                Cedula = model.Cedula!,
                Mail = model.Email!,
                Contra = model.Contra!, // ⚠️ En producción, hashear la contraseña
                Sexo = model.Sexo!.Value,
                Nacionalidad = model.Nacionalidad!
            };

            _context.Corredor.Add(corredor);
            await _context.SaveChangesAsync();

            // Autenticar al nuevo usuario
            await AutenticarUsuario(corredor);

            // Crear inscripción
            var numeroCorredor = await GenerarNumeroCorredor(model.CarreraId);
            var inscripcion = new Inscripcion
            {
                CorredorId = corredor.Id,
                CarreraId = model.CarreraId,
                NumeroCorredor = numeroCorredor,
                Estado = EstadoInscripcion.Confirmada,
                FechaInscripcion = DateTime.UtcNow,
            };

            _context.Inscripcion.Add(inscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "¡Cuenta creada e inscripción realizada con éxito!";
            return RedirectToAction("Details", "Carreras", new { id = model.CarreraId });
        }

        private async Task<int> GenerarNumeroCorredor(int carreraId)
        {
            var ultimoNumero = await _context.Inscripcion
                .Where(i => i.CarreraId == carreraId)
                .OrderByDescending(i => i.NumeroCorredor)
                .Select(i => i.NumeroCorredor)
                .FirstOrDefaultAsync();

            return ultimoNumero + 1;
        }

        private async Task AutenticarUsuario(Corredor corredor)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, corredor.Id.ToString()),
                new Claim(ClaimTypes.Name, corredor.NombreCompleto),
                new Claim(ClaimTypes.Email, corredor.Mail),
                new Claim(ClaimTypes.Role, "Corredor")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // GET: Inscripcion/GestionarParticipantes/5
        public async Task<IActionResult> GestionarParticipantes(int carreraId)
        {
            var carrera = await _context.Carrera
                .Include(c => c.Inscripciones)
                    .ThenInclude(i => i.Corredor)
                .FirstOrDefaultAsync(c => c.Id == carreraId);

            if (carrera == null)
            {
                return NotFound();
            }

            ViewData["Carrera"] = carrera;

            var inscripciones = await _context.Inscripcion
                .Where(i => i.CarreraId == carreraId)
                .Include(i => i.Corredor)
                .Include(i => i.Registro)
                .OrderBy(i => i.NumeroCorredor)
                .ToListAsync();

            return View(inscripciones);
        }

        // POST: Inscripcion/CambiarEstadoInscripcion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoInscripcion(int inscripcionId, EstadoInscripcion nuevoEstado)
        {
            var inscripcion = await _context.Inscripcion
                .Include(i => i.Carrera)
                .FirstOrDefaultAsync(i => i.Id == inscripcionId);

            if (inscripcion == null)
            {
                return NotFound();
            }

            inscripcion.Estado = nuevoEstado;
            _context.Update(inscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Estado de inscripción actualizado a {nuevoEstado}";
            return RedirectToAction("GestionarParticipantes", new { carreraId = inscripcion.CarreraId });
        }

        // GET: Inscripcion/VerificarInscripcion/5
        [HttpGet]
        public async Task<IActionResult> VerificarInscripcion(int carreraId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { yaInscrito = false, mensaje = "Usuario no autenticado" });
            }

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var yaInscrito = await _context.Inscripcion
                .AnyAsync(i => i.CorredorId == usuarioId &&
                              i.CarreraId == carreraId &&
                              i.Estado != EstadoInscripcion.Cancelada);

            return Json(new
            {
                yaInscrito = yaInscrito,
                mensaje = yaInscrito ? "Ya estás inscrito en esta carrera" : "No estás inscrito"
            });
        }

        // POST: Inscripcion/EliminarInscripcion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarInscripcion(int inscripcionId)
        {
            var inscripcion = await _context.Inscripcion
                .Include(i => i.Carrera)
                .FirstOrDefaultAsync(i => i.Id == inscripcionId);

            if (inscripcion == null)
            {
                return NotFound();
            }

            var carreraId = inscripcion.CarreraId;

            _context.Inscripcion.Remove(inscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Inscripción eliminada correctamente";
            return RedirectToAction("GestionarParticipantes", new { carreraId });
        }
    }
}