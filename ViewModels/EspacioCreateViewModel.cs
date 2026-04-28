using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeportivoApp.ViewModels
{
    public class EspacioCreateViewModel
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public string Estado { get; set; }

        public List<SelectListItem> Usuarios { get; set; } = new();
    }
}