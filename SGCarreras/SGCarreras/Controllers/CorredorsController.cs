using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SGCarreras.Models.Sexo;

namespace SGCarreras.Controllers
{
    public class CorredorsController : Controller
    {
        private readonly SGCarrerasContext _context;

        public CorredorsController(SGCarrerasContext context)
        {
            _context = context;
        }

        // GET: Corredors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Corredor.ToListAsync());
        }

        // GET: Corredors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corredor = await _context.Corredor
                .FirstOrDefaultAsync(m => m.id == id);
            if (corredor == null)
            {
                return NotFound();
            }

            return View(corredor);
        }

        // GET: Corredors/Create
        public IActionResult Create()
        {
            ViewBag.SexoList = Enum.GetValues(typeof(SexoEnum)) // <-- el tipo del enum
                            .Cast<SexoEnum>()
                            .Select(s => new SelectListItem
                            {
                                Value = s.ToString(),
                                Text = s.ToString()
                            }).ToList();


            return View();
        }

        // POST: Corredors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("sexo,nombreCompleto,cedula")] Corredor corredor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(corredor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 🔸 Agrega el error al ModelState para que aparezca en la vista
                    ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
                }
            }

            // 🔸 Si llega aquí, algo falló. Vuelve a mostrar la vista con los errores
            return View(corredor);
        }

        // GET: Corredors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corredor = await _context.Corredor.FindAsync(id);
            if (corredor == null)
            {
                return NotFound();
            }
            return View(corredor);
        }

        // POST: Corredors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("sexo,id,nombreCompleto,cedula")] Corredor corredor)
        {
            if (id != corredor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(corredor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorredorExists(corredor.id))
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
            return View(corredor);
        }

        // GET: Corredors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corredor = await _context.Corredor
                .FirstOrDefaultAsync(m => m.id == id);
            if (corredor == null)
            {
                return NotFound();
            }

            return View(corredor);
        }

        // POST: Corredors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var corredor = await _context.Corredor.FindAsync(id);
            if (corredor != null)
            {
                _context.Corredor.Remove(corredor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorredorExists(int id)
        {
            return _context.Corredor.Any(e => e.id == id);
        }
    }
}
