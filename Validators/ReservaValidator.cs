using DeportivoApp.Models;

namespace DeportivoApp.Validators
{
    public static class ReservaValidator
    {
        public static (bool IsValid, string Message) Validar(Reserva reserva)
        {
            if (reserva.UsuarioId <= 0)
                return (false, "Usuario inválido");

            if (reserva.EspacioId <= 0)
                return (false, "Espacio inválido");

            if (reserva.HoraInicio >= reserva.HoraFin)
                return (false, "La hora de inicio debe ser menor a la hora fin");

            return (true, "OK");
        }
    }
}