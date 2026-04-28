using DeportivoApp.Models;

namespace DeportivoApp.Services.Interfaces
{
    public interface IEspacioService
    {
        Task<(bool Success, string Message)> CreateAsync(Espacio espacio);
        Task<List<Espacio>> GetAllAsync();
        Task<Espacio?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> UpdateAsync(Espacio espacio);
        Task<bool> DeleteAsync(int id);
        Task<bool> CambiarEstadoAsync(int espacioId, string estado);
    }
}
