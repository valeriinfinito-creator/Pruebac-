namespace DeportivoApp.Services.Interfaces;

public interface INotificacionService
{
    Task EnviarConfirmacionReservaAsync(int reservaId);
}