using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SGCarreras.Models.Estado;
using static SGCarreras.Models.Sexo;

namespace SGCarreras.Controllers
{
    public class CarrerasController : Controller
    {
        private readonly SGCarrerasContext _context;

        public CarrerasController(SGCarrerasContext context)
        {
            _context = context;
        }

        // GET: Carreras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carrera.ToListAsync());
        }

        // GET: Carreras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carrera
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
            ViewBag.EstadoList = Enum.GetValues(typeof(EstadoEnum)) // <-- el tipo del enum
                            .Cast<EstadoEnum>()
                            .Select(s => new SelectListItem
                            {
                                Value = s.ToString(),
                                Text = s.ToString()
                            }).ToList();



            return View();
        }

        // POST: Carreras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,ubicacion,estado,kmTotales")] Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carrera);
        }

        // GET: Carreras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carrera.FindAsync(id);
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


        // POST: Carreras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,ubicacion,estado,kmTotales")] Carrera carrera)
        {
            if (id != carrera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrera);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
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
            var carrera = await _context.Carrera.FindAsync(id);
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
                .Where(c => c.Estado == EstadoEnum.Activo)
                .ToListAsync();
            return View(carrerasActivas);
        }

        // Listado de Carreras En Espera
        public async Task<IActionResult> ListadoEnEspera()
        {
            var carrerasEnEspera = await _context.Carrera
                .Where(c => c.Estado == EstadoEnum.En_espera)
                .ToListAsync();
            return View(carrerasEnEspera);
        }

        // Listado de Carreras Finalizadas
        public async Task<IActionResult> ListadoFinalizadas()
        {
            var carrerasFinalizadas = await _context.Carrera
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
            return View(carreras);
        }


    }
}
