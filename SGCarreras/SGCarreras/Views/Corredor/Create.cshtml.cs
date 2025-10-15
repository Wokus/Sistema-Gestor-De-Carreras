using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGCarreras.Data;
using SGCarreras.Models;

namespace SGCarreras.Views.Corredor
{
    public class CreateModel : PageModel
    {
        private readonly SGCarrerasContext _context;

        public CreateModel(SGCarrerasContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Corredor Corredor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Corredor.Add(Corredor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
