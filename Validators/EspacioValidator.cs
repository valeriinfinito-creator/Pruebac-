using DeportivoApp.Models;

namespace DeportivoApp.Validators
{
    public static class EspacioValidator
    {
        public static (bool IsValid, string Message) Validar(Espacio espacio)
        {
            if (string.IsNullOrWhiteSpace(espacio.Nombre))
                return (false, "El nombre es obligatorio");

            if (espacio.HoraInicio >= espacio.HoraFin)
                return (false, "Horario inválido");

            return (true, "OK");
        }
    }
}