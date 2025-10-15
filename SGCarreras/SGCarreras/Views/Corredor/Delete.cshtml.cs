using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;

namespace SGCarreras.Views.Corredor
{
    public class DeleteModel : PageModel
    {
        private readonly SGCarrerasContext _context;

        public DeleteModel(SGCarrerasContext context)
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

            var corredor = await _context.Corredor.FirstOrDefaultAsync(m => m.id == id);

            if (corredor == null)
            {
                return NotFound();
            }
            else
            {
                Corredor = corredor;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corredor = await _context.Corredor.FindAsync(id);
            if (corredor != null)
            {
                Corredor = corredor;
                _context.Corredor.Remove(Corredor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
