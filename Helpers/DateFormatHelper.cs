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
        public static bool EsRangoValido(TimeSpan inicio, TimeSpan fin)
        {
            return fin > inicio;
        }

        // Validar cruce de horarios
        public static bool HayCruce(TimeSpan inicio1, TimeSpan fin1, TimeSpan inicio2, TimeSpan fin2)
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