using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using SGCarreras.Models.dtos;
using SGCarreras.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static SGCarreras.Models.Estado;
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
        public async Task<IActionResult> Details(int? id, string filtro = "Todas")
        {
            if (id == null)
            {
                return NotFound();
            }
            var corredor = await _context.Corredor
                        .Include(m => m.registros)
                            .ThenInclude(r => r.Carrera)
                            .FirstOrDefaultAsync(m => m.Id == id);

            var inscri = await _context.Inscripcion
                        .Include(m => m.Corredor)
                          .Include(m => m.Carrera).Where(m => m.Corredor.Id == id)
                            .ToListAsync();

            if (corredor == null)
            {
                return NotFound();
            }
            CorredorDTO correDto = new CorredorDTO();
            correDto.Id = corredor.Id;
            correDto.Cedula = corredor.Cedula;
            correDto.NombreCompleto = corredor.NombreCompleto;
            correDto.Nacionalidad = corredor.Nacionalidad;
            correDto.Mail = corredor.Mail;
            correDto.Registros = inscri;
            List<Carrera> carrerasDeUsuario = new List<Carrera>();

            foreach (var item in corredor.registros)
            {
                carrerasDeUsuario.Add(item.Carrera);
            }

            ViewBag.FiltroActual = filtro;


            return View(correDto);
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
        public async Task<IActionResult> Create([Bind("Sexo,NombreCompleto,Cedula, Contra, Mail")] Corredor corredor)
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
        public async Task<IActionResult> Registrar([Bind("Sexo,NombreCompleto,Cedula, Nacionalidad,Contra,Mail")] Corredor corredor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(corredor);
                    await _context.SaveChangesAsync();

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, corredor.Mail),
                new Claim(ClaimTypes.Name, corredor.NombreCompleto),
                new Claim(ClaimTypes.NameIdentifier, corredor.Id.ToString()),
                new Claim(ClaimTypes.Role, "Corredor"),
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");

                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error desde la base de datos: {ex.Message}");
                    if (ex.InnerException?.Message.Contains("duplicate") == true)
                    {
                        ModelState.AddModelError("Mail", "Este correo electrónico ya está registrado.");
                    }
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
        public async Task<IActionResult> LogIn([Bind("Mail,Contra")] CorredorLogIn corredorLogIn)
        {
            if (ModelState.IsValid)
            {
                Usuario usr = new Usuario();
                usr.Contra = corredorLogIn.Contra;
                usr.Mail = corredorLogIn.Mail;

                try
                {
                    var corre = _context.Corredor.Where(x => x.Mail == usr.Mail && x.Contra == usr.Contra).FirstOrDefault();
                    if (corre != null)
                    {

                        var claims = new List<Claim>
                        {
                        new Claim (ClaimTypes.Email, corre.Mail),
                        new Claim (ClaimTypes.Name, corre.NombreCompleto),
                        new Claim (ClaimTypes.NameIdentifier, corre.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Corredor"),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var admin = _context.Administrador.Where(x => x.Mail == usr.Mail && x.Contra == usr.Contra).FirstOrDefault();

                        if (admin != null) {
                            var claims = new List<Claim>
                            {
                            new Claim (ClaimTypes.Email, admin.Mail),
                            new Claim (ClaimTypes.Name, admin.NombreCompleto),
                            new Claim (ClaimTypes.NameIdentifier, admin.Id.ToString()),
                            new Claim(ClaimTypes.Role, "Administrador"),
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
        public async Task<IActionResult> Edit(int id, [Bind("Sexo,Id,NombreCompleto,Cedula,Nacionalidad, Mail, Contra")] Corredor corredor)
        {
            if (id != corredor.Id)
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
                    if (!CorredorExists(corredor.Id))
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (corredor == null)
            {
                return NotFound();
            }

            return View(corredor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Usando SQL directo para evitar completamente los problemas de FK
                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Inscripcion WHERE CorredorId = {0}", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Registro WHERE CorredorId = {0}", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Corredor WHERE Id = {0}", id);

                TempData["Success"] = "Corredor eliminado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el corredor: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error eliminando corredor: {ex}");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CorredorExists(int id)
        {
            return _context.Corredor.Any(e => e.Id == id);
        }        
    }
}
