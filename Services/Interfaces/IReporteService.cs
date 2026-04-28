using DeportivoApp.ViewModels;

namespace DeportivoApp.Services.Interfaces
{
    public interface IReporteService
    {
        Task<object> GetDashboardAsync();

        Task<List<ReporteViewModel>> GetEspacioConMasReservasAsync();
        Task<List<ReporteViewModel>> GetUsuariosMasAtendidosAsync();
        Task<List<ReporteViewModel>> GetEspaciosMasUsadosAsync();
        Task<double> GetTasaInasistenciaAsync();
        Task<List<ReporteViewModel>> GetReportesAsync();
    }
}