using Microsoft.EntityFrameworkCore;
using DeportivoApp.Data;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.ViewModels;

namespace DeportivoApp.Services.Implementations
{
    public class ReporteService : IReporteService
    {
        private readonly MySqlDBContext _context;

        public ReporteService(MySqlDBContext context)
        {
            _context = context;
        }

        // Dashboard general
        public async Task<object> GetDashboardAsync()
        {
            return new
            {
                EspacioConMasReservas = await GetEspacioConMasReservasAsync(),
                UsuariosTop = await GetUsuariosMasAtendidosAsync(),
                EspaciosMasUsados = await GetEspaciosMasUsadosAsync(),
                TasaInasistencia = await GetTasaInasistenciaAsync()
            };
        }

        // Reporte general (reservas por espacio)
        public async Task<List<ReporteViewModel>> GetReportesAsync()
        {
            return await _context.Reservas
                .Include(r => r.Espacio)
                .GroupBy(r => r.Espacio.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();
        }

        // Espacio con más reservas
        public async Task<List<ReporteViewModel>> GetEspacioConMasReservasAsync()
        {
            return await _context.Reservas
                .Include(r => r.Espacio)
                .GroupBy(r => r.Espacio.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        // Usuarios más activos (atendidos)
        public async Task<List<ReporteViewModel>> GetUsuariosMasAtendidosAsync()
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Where(r => r.Estado == "Atendida")
                .GroupBy(r => r.Usuario.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        // Espacios más usados
        public async Task<List<ReporteViewModel>> GetEspaciosMasUsadosAsync()
        {
            return await _context.Reservas
                .Include(r => r.Espacio)
                .GroupBy(r => r.Espacio.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        // Tasa de inasistencia
        public async Task<double> GetTasaInasistenciaAsync()
        {
            var total = await _context.Reservas.CountAsync();
            if (total == 0) return 0;

            var inasistencias = await _context.Reservas
                .CountAsync(r => r.Estado == "No asistió");

            return (double)inasistencias / total * 100;
        }
    }
}