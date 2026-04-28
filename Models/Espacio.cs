using System.ComponentModel.DataAnnotations;

namespace DeportivoApp.Models;

public class Espacio
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(191)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(191)]
    public string Tipo { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue)]
    public int Capacidad { get; set; }

    // RELACIÓN CON RESERVAS
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [MaxLength(50)]
    public string Estado { get; set; } = "Disponible";
}
