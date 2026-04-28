namespace VeterinariaApp.Helpers
{
    public static class DateHelper
    {
        // Validar que no sea fecha pasada
        public static bool EsFechaPasada(DateTime fecha)
        {
            return fecha.Date < DateTime.Now.Date;
        }

        // Validar rango de horas
        public static bool EsRangoValido(DateTime inicio, DateTime fin)
        {
            return fin > inicio;
        }

        // Validar cruce de horarios
        public static bool HayCruce(DateTime inicio1, DateTime fin1, DateTime inicio2, DateTime fin2)
        {
            return inicio1 < fin2 && fin1 > inicio2;
        }

        // Obtener solo fecha (sin hora)
        public static DateTime SoloFecha(DateTime fecha)
        {
            return fecha.Date;
        }

        // Obtener solo hora
        public static TimeSpan SoloHora(DateTime fecha)
        {
            return fecha.TimeOfDay;
        }
    }
}