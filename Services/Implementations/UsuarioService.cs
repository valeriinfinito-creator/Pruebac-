using Microsoft.EntityFrameworkCore;
using DeportivoApp.Data;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;

namespace DeportivoApp.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly MySqlDBContext _context;

        public UsuarioService(MySqlDBContext context)
        {
            _context = context;
        }

        // Get all
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Espacios)
                .ToListAsync();
        }

        // Get by id
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Espacios)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Create
        public async Task<(bool Success, string Message)> CreateAsync(Usuario usuario)
        {
            if (await ExisteDocumentoAsync(usuario.Documento))
                return (false, "Ya existe un usuario con ese documento");

            if (await ExisteEmailAsync(usuario.Email))
                return (false, "Ya existe un usuario con ese email");

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return (true, "Creado correctamente");
        }

        // Update
        public async Task<(bool Success, string Message)> UpdateAsync(Usuario usuario)
        {
            var existente = await _context.Usuarios.FindAsync(usuario.Id);

            if (existente == null)
                return (false, "Usuario no encontrado");

            _context.Entry(existente).CurrentValues.SetValues(usuario);
            await _context.SaveChangesAsync();

            return (true, "Actualizado correctamente");
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        // Validaciones
        public async Task<bool> ExisteDocumentoAsync(string documento)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Documento == documento);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email);
        }
    }
}