using System.ComponentModel.DataAnnotations;

namespace DeportivoApp.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Documento { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    // RELACIONES
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    // 🔧 SOLUCIÓN ERROR NombreCompleto
    public string NombreCompleto => Nombre;
}
