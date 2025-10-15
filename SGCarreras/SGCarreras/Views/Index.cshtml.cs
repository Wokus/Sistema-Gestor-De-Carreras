using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;

namespace SGCarreras.Views
{
    public class IndexModel : PageModel
    {
        private readonly SGCarreras.Data.SGCarrerasContext _context;

        public IndexModel(SGCarreras.Data.SGCarrerasContext context)
        {
            _context = context;
        }

        public IList<Corredor> Corredor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Corredor = await _context.Corredor.ToListAsync();
        }
    }
}
