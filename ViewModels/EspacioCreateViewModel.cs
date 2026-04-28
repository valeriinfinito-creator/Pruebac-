using System.ComponentModel.DataAnnotations;

namespace DeportivoApp.ViewModels
{
    public class EspacioCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Capacidad { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}
