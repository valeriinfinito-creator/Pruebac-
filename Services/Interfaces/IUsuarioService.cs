using DeportivoApp.Models;

namespace DeportivoApp.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Usuario usuario);
        Task<(bool Success, string Message)> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);

        Task<bool> ExisteDocumentoAsync(string documento);
        Task<bool> ExisteEmailAsync(string email);
    }
}
