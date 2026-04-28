using Microsoft.EntityFrameworkCore;
using DeportivoApp.Data;
using DeportivoApp.Helpers;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.Validators; 

namespace DeportivoApp.Services.Implementations
{
    public class ReservaService : IReservaService
    {
        private readonly MySqlDBContext _context;
        private readonly EmailHelper _email;

        public ReservaService(MySqlDBContext context, EmailHelper email)
        {
            _context = context;
            _email = email;
        }

        // CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Reserva reserva)
        {
            try
            {
                var validacion = ReservaValidator.Validar(reserva);

                if (!validacion.IsValid)
                    return (false, validacion.Message);

                if (await TieneReservasActivasAsync(reserva.UsuarioId))
                    return (false, "El usuario ya tiene 2 reservas activas");

                if (await EstaBloqueadaAsync(reserva.UsuarioId))
                    return (false, "Usuario bloqueado por inasistencias");

                if (await HayConflictoHorarioAsync(
                    reserva.UsuarioId,
                    reserva.Fecha,
                    reserva.HoraInicio,
                    reserva.HoraFin))
                    return (false, "El usuario ya tiene una reserva en ese horario");

                reserva.Estado = "Programada";

                await _context.Reservas.AddAsync(reserva);
                await _context.SaveChangesAsync();

                return (true, "Reserva creada correctamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        // MÉTODO DE INTERFAZ
        public async Task<List<Reserva>> GetReservasAsync()
        {
            return await _context.Reservas.ToListAsync();
        }

        // GET ALL
        public async Task<List<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Espacio)
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        // GET BY ID
        public async Task<Reserva?> GetByIdAsync(int id) 
        {
            return await _context.Reservas
                .Include(r => r.Espacio)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // UPDATE
        public async Task<(bool Success, string Message)> UpdateAsync(Reserva reserva)
        {
            var existente = await _context.Reservas.FindAsync(reserva.Id);
            if (existente == null)
                return (false, "Reserva no encontrada");

            _context.Entry(existente).CurrentValues.SetValues(reserva);
            await _context.SaveChangesAsync();

            return (true, "Reserva actualizada");
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }

        // CAMBIAR ESTADO
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return false;

            reserva.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        // BLOQUEO
        public async Task<bool> EstaBloqueadaAsync(int usuarioId)
        {
            var inasistencias = await ContarInasistenciasAsync(usuarioId);
            return inasistencias >= 3;
        }

        public async Task<int> ContarInasistenciasAsync(int usuarioId)
        {
            return await _context.Reservas
                .CountAsync(r => r.UsuarioId == usuarioId &&
                                 r.Estado == "No asistió");
        }

        // VALIDAR CONFLICTO
        public async Task<bool> HayConflictoHorarioAsync(
            int usuarioId,
            DateTime fecha,
            DateTime inicio,
            DateTime fin,
            int? reservaId = null)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.UsuarioId == usuarioId &&
                r.Fecha.Date == fecha.Date &&
                r.Id != reservaId &&
                inicio < r.HoraFin &&
                fin > r.HoraInicio
            );
        }

        // VALIDAR LIMITE
        public async Task<bool> TieneReservasActivasAsync(int usuarioId)
        {
            var count = await _context.Reservas
                .CountAsync(r => r.UsuarioId == usuarioId &&
                                 r.Estado == "Programada");

            return count >= 2;
        }
    }
}
        

