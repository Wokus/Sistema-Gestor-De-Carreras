using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;

namespace SGCarreras.Views.Corredor
{
    public class EditModel : PageModel
    {
        private readonly SGCarrerasContext _context;

        public EditModel(SGCarrerasContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Corredor Corredor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corredor =  await _context.Corredor.FirstOrDefaultAsync(m => m.id == id);
            if (corredor == null)
            {
                return NotFound();
            }
            Corredor = corredor;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Corredor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorredorExists(Corredor.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CorredorExists(string id)
        {
            return _context.Corredor.Any(e => e.id == id);
        }
    }
}
