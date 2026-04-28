using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeportivoApp.ViewModels
{
    public class ReservaCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EspacioId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }

        public string Estado { get; set; } = "Programada";

        public List<SelectListItem> Usuarios { get; set; } = new();
        public List<SelectListItem> Espacios { get; set; } = new();
    }
}