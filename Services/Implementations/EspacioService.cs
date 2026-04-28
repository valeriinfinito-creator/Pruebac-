using Microsoft.EntityFrameworkCore;
using DeportivoApp.Data;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.Validators; 

namespace DeportivoApp.Services.Implementations
{
    public class EspacioService : IEspacioService
    {
        private readonly MySqlDBContext _context;

        public EspacioService(MySqlDBContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Espacio espacio)
        {
            try
            {
                var validacion = EspacioValidator.Validar(espacio);
                if (!validacion.IsValid)
                    return (false, validacion.Message);

                espacio.Nombre = espacio.Nombre.Trim();
                espacio.Tipo = espacio.Tipo.Trim();

                var existeDuplicado = await _context.Espacios.AnyAsync(e =>
                    e.Nombre == espacio.Nombre && e.Tipo == espacio.Tipo
                );

                if (existeDuplicado)
                    return (false, "Ya existe un espacio con ese nombre y tipo");

                if (string.IsNullOrWhiteSpace(espacio.Estado))
                    espacio.Estado = "Disponible";

                await _context.Espacios.AddAsync(espacio); 
                await _context.SaveChangesAsync();

                return (true, "Espacio creado correctamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        // GET ALL
        public async Task<List<Espacio>> GetAllAsync()
        {
            return await _context.Espacios
                .Include(e => e.Reservas)
                .ToListAsync();
        }

        // GET BY ID
        public async Task<Espacio?> GetByIdAsync(int id)
        {
            return await _context.Espacios
                .Include(e => e.Reservas)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // UPDATE
        public async Task<(bool Success, string Message)> UpdateAsync(Espacio espacio)
        {
            var existente = await _context.Espacios.FindAsync(espacio.Id);
            if (existente == null)
                return (false, "Espacio no encontrado");

            var validacion = EspacioValidator.Validar(espacio);
            if (!validacion.IsValid)
                return (false, validacion.Message);

            var nombre = espacio.Nombre.Trim();
            var tipo = espacio.Tipo.Trim();

            var existeDuplicado = await _context.Espacios.AnyAsync(e =>
                e.Id != espacio.Id &&
                e.Nombre == nombre &&
                e.Tipo == tipo
            );

            if (existeDuplicado)
                return (false, "Ya existe un espacio con ese nombre y tipo");

            espacio.Nombre = nombre;
            espacio.Tipo = tipo;

            _context.Entry(existente).CurrentValues.SetValues(espacio);
            await _context.SaveChangesAsync();

            return (true, "Espacio actualizado");
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null) return false;

            _context.Espacios.Remove(espacio);
            await _context.SaveChangesAsync();
            return true;
        }

        // CAMBIAR ESTADO
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null) return false;

            espacio.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
