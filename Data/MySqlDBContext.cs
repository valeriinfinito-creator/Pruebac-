using Microsoft.EntityFrameworkCore;
using DeportivoApp.Models;

namespace DeportivoApp.Data
{
    public class MySqlDBContext : DbContext
    {
        public MySqlDBContext(DbContextOptions<MySqlDBContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Espacio> Espacios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Documento)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Espacio>()
                .Property(e => e.Nombre)
                .HasMaxLength(191);

            modelBuilder.Entity<Espacio>()
                .Property(e => e.Tipo)
                .HasMaxLength(191);

            modelBuilder.Entity<Espacio>()
                .HasIndex(e => new { e.Nombre, e.Tipo })
                .IsUnique();

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reserva → Espacio
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Espacio)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.EspacioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
