using Microsoft.EntityFrameworkCore;
using DeportivoApp.Data;
using DeportivoApp.Helpers;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.Validators; 

namespace DeportivoApp.Services.Implementations
{
    public class EspacioService : IEspacioService
    {
        private readonly MySqlDBContext _context;
        private readonly EmailHelper _email;

        public EspacioService(MySqlDBContext context, EmailHelper email)
        {
            _context = context;
            _email = email;
        }

        // CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Espacio espacio)
        {
            try
            {
                var validacion = EspacioValidator.Validar(espacio);
                if (!validacion.IsValid)
                    return (false, validacion.Message);

                if (await TieneEspaciosActivasAsync(espacio.UsuarioId))
                    return (false, "El usuario ya tiene 2 espacios activos");

                if (await EstaBloqueadaAsync(espacio.UsuarioId))
                    return (false, "Usuario bloqueado por inasistencias");

                if (await HayConflictoHorarioAsync(
                    espacio.UsuarioId,
                    espacio.Fecha,
                    espacio.HoraInicio,
                    espacio.HoraFin))
                    return (false, "Conflicto de horario");

                espacio.Estado = "Programada";

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
                .Include(e => e.Usuario)
                .ToListAsync();
        }

        // GET BY ID
        public async Task<Espacio?> GetByIdAsync(int id)
        {
            return await _context.Espacios
                .Include(e => e.Reservas)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // UPDATE
        public async Task<(bool Success, string Message)> UpdateAsync(Espacio espacio)
        {
            var existente = await _context.Espacios.FindAsync(espacio.Id);
            if (existente == null)
                return (false, "Espacio no encontrado");

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

        // USUARIOS
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // BLOQUEO
        public async Task<bool> EstaBloqueadaAsync(int usuarioId)
        {
            var inasistencias = await ContarInasistenciasAsync(usuarioId);
            return inasistencias >= 3;
        }

        public async Task<int> ContarInasistenciasAsync(int usuarioId)
        {
            return await _context.Espacios
                .CountAsync(e => e.UsuarioId == usuarioId &&
                                 e.Estado == "No asistió");
        }

        // VALIDAR CONFLICTO
        public async Task<bool> HayConflictoHorarioAsync(
            int usuarioId,
            DateTime fecha,
            DateTime inicio,
            DateTime fin,
            int? espacioId = null)
        {
            return await _context.Espacios.AnyAsync(e =>
                e.UsuarioId == usuarioId &&
                e.Fecha.Date == fecha.Date &&
                e.Id != espacioId &&
                inicio < e.HoraFin &&
                fin > e.HoraInicio
            );
        }

        // VALIDAR LIMITE
        public async Task<bool> TieneEspaciosActivasAsync(int usuarioId)
        {
            var count = await _context.Espacios
                .CountAsync(e => e.UsuarioId == usuarioId &&
                                 e.Estado == "Programada");

            return count >= 2;
        }
    }
} 