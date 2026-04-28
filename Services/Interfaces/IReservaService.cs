using DeportivoApp.Models;

namespace DeportivoApp.Services.Interfaces
{
    public interface IReservaService
    {
        Task<(bool Success, string Message)> CreateAsync(Reserva reserva);
        Task<List<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> UpdateAsync(Reserva reserva);
        Task<bool> DeleteAsync(int id);
        Task<bool> CambiarEstadoAsync(int reservaId, string estado);
        Task<List<Reserva>> GetReservasAsync();
    }
}
