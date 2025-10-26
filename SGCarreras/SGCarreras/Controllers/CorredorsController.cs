using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SGCarreras.Models.ViewModels;
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
                
                    ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
                }
            }

            return View(corredor);
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar([Bind("sexo,nombreCompleto,cedula,contra,mail")] Corredor corredor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(corredor);
                    await _context.SaveChangesAsync();


                    ViewBag.Message = $"{corredor.nombreCompleto} / {corredor.mail}, Registrado correcramente.";
                    ModelState.Clear();


                    return RedirectToAction(nameof(Index));

                    
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error desde la base de datos: {ex.Message}");
                    //error, gmail ya ingresado.
                    return View(corredor);
                }
            }

            return View(corredor);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("mail,contra")] CorredorLogIn corredorLogIn)
        {
            if (ModelState.IsValid)
            {
                Corredor corredor = new Corredor();
                corredor.contra = corredorLogIn.contra;
                corredor.mail = corredorLogIn.mail;

                try
                {
                    var corre = _context.Corredor.Where(x => x.mail == corredor.mail && x.contra == corredor.contra).FirstOrDefault();
                    if (corre != null)
                    {

                        var claims = new List<Claim>
                    {
                    new Claim (ClaimTypes.Name, corre.mail),
                    new Claim(ClaimTypes.Role, "Corredor"),
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mail o Contraseña incorrecta/s");
                    }

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
                }
                   
            }

            return View(corredorLogIn);
        }
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

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
