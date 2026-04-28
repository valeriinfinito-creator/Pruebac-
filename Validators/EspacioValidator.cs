using DeportivoApp.Models;

namespace DeportivoApp.Validators
{
    public static class EspacioValidator
    {
        public static (bool IsValid, string Message) Validar(Espacio espacio)
        {
            if (string.IsNullOrWhiteSpace(espacio.Nombre))
                return (false, "El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(espacio.Tipo))
                return (false, "El tipo es obligatorio");

            if (espacio.Capacidad <= 0)
                return (false, "La capacidad debe ser mayor a 0");

            return (true, "OK");
        }
    }
}
