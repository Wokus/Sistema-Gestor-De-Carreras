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
        public DbSet<SGCarreras.Models.PuntoDeControl> PuntosDeControl { get; set; } = default!;
        public DbSet<SGCarreras.Models.Registro> Registro { get; set; } = default!;
        public DbSet<SGCarreras.Models.Inscripcion> Inscripcion { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Registro)
                .WithMany()
                .HasForeignKey(i => i.RegistroId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registro>()
                .HasOne(r => r.Corredor)
                .WithMany(c => c.registros)
                .HasForeignKey(r => r.CorredorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Corredor)
                .WithMany()
                .HasForeignKey(i => i.CorredorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
