using DeportivoApp.Data;
using DeportivoApp.Helpers;
using DeportivoApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeportivoApp.Services.Implementations;

public class NotificacionService : INotificacionService
{
    private readonly MySqlDBContext _context;
    private readonly EmailHelper _emailHelper;

    public NotificacionService(MySqlDBContext context, EmailHelper emailHelper)
    {
        _context = context;
        _emailHelper = emailHelper;
    }

    public async Task EnviarConfirmacionReservaAsync(int reservaId)
    {
        try
        {
            var reserva = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Espacio)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == reservaId);

            if (reserva == null || reserva.Usuario == null || reserva.Espacio == null)
            {
                throw new InvalidOperationException("No se encontró la reserva con sus relaciones");
            }

            await _emailHelper.EnviarConfirmacionReservaAsync(
                reserva.Usuario.Email,
                reserva.Usuario.Nombre,
                reserva.Espacio.Nombre,
                reserva.Espacio.HoraInicio   
            );
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al enviar notificación de reserva", ex);
        }
    }
}