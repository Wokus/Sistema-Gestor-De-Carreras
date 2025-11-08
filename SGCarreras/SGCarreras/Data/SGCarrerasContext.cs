using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGCarreras.Models;

namespace SGCarreras.Data
{
    public class SGCarrerasContext : DbContext
    {
        public SGCarrerasContext (DbContextOptions<SGCarrerasContext> options)
            : base(options)
        {
        }
        public DbSet<SGCarreras.Models.Corredor> Corredor { get; set; } = default!;
        public DbSet<Administradoor> Administrador { get; set; }
        public DbSet<SGCarreras.Models.Carrera> Carrera { get; set; } = default!;
        public DbSet<SGCarreras.Models.Registro> Registro { get; set; } = default!;
    }
}
