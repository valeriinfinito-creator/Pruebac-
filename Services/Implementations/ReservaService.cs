using Microsoft.EntityFrameworkCore;
using DeportivoApp.Data;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.Validators;

namespace DeportivoApp.Services.Implementations
{
    public class ReservaService : IReservaService
    {
        private readonly MySqlDBContext _context;
        private readonly INotificacionService _notificacionService;

        public ReservaService(MySqlDBContext context, INotificacionService notificacionService)
        {
            _context = context;
            _notificacionService = notificacionService;
        }

        // CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Reserva reserva)
        {
            try
            {
                var validacion = ReservaValidator.Validar(reserva);

                if (!validacion.IsValid)
                    return (false, validacion.Message);

                if (EsFechaHoraPasada(reserva.Fecha, reserva.HoraInicio))
                    return (false, "No se pueden crear reservas en fechas u horas pasadas");

                if (await HayConflictoHorarioAsync(
                    reserva.UsuarioId,
                    reserva.Fecha,
                    reserva.HoraInicio,
                    reserva.HoraFin))
                    return (false, "El usuario ya tiene una reserva en ese horario");

                if (await HayConflictoEspacioAsync(
                    reserva.EspacioId,
                    reserva.Fecha,
                    reserva.HoraInicio,
                    reserva.HoraFin))
                    return (false, "El espacio ya tiene una reserva en ese rango de horario");

                reserva.Estado = "Programada";

                await _context.Reservas.AddAsync(reserva);
                await _context.SaveChangesAsync();

                var mensaje = "Reserva creada correctamente";

                try
                {
                    await _notificacionService.EnviarConfirmacionReservaAsync(reserva.Id);
                }
                catch
                {
                    mensaje = "Reserva creada correctamente (no se pudo enviar el correo)";
                }

                return (true, mensaje);
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

            var validacion = ReservaValidator.Validar(reserva);
            if (!validacion.IsValid)
                return (false, validacion.Message);

            if (EsFechaHoraPasada(reserva.Fecha, reserva.HoraInicio))
                return (false, "No se pueden dejar reservas en fechas u horas pasadas");

            if (await HayConflictoHorarioAsync(
                    reserva.UsuarioId,
                    reserva.Fecha,
                    reserva.HoraInicio,
                    reserva.HoraFin,
                    reserva.Id))
                return (false, "El usuario ya tiene una reserva en ese horario");

            if (await HayConflictoEspacioAsync(
                    reserva.EspacioId,
                    reserva.Fecha,
                    reserva.HoraInicio,
                    reserva.HoraFin,
                    reserva.Id))
                return (false, "El espacio ya tiene una reserva en ese rango de horario");

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

        // VALIDAR CONFLICTO
        public async Task<bool> HayConflictoHorarioAsync(
            int usuarioId,
            DateTime fecha,
            TimeSpan inicio,
            TimeSpan fin,
            int? reservaId = null)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.UsuarioId == usuarioId &&
                r.Fecha.Date == fecha.Date &&
                r.Id != reservaId &&
                r.Estado == "Programada" &&
                inicio < r.HoraFin &&
                fin > r.HoraInicio
            );
        }

        private async Task<bool> HayConflictoEspacioAsync(
            int espacioId,
            DateTime fecha,
            TimeSpan inicio,
            TimeSpan fin,
            int? reservaId = null)
        {
            return await _context.Reservas.AnyAsync(r =>
                r.EspacioId == espacioId &&
                r.Fecha.Date == fecha.Date &&
                r.Id != reservaId &&
                r.Estado == "Programada" &&
                inicio < r.HoraFin &&
                fin > r.HoraInicio
            );
        }

        private static bool EsFechaHoraPasada(DateTime fecha, TimeSpan horaInicio)
        {
            var inicio = fecha.Date.Add(horaInicio);
            return inicio < DateTime.Now;
        }
    }
}


