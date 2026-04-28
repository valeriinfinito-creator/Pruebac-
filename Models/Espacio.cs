using System.ComponentModel.DataAnnotations;

namespace DeportivoApp.Models;

public class Espacio
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFin { get; set; }

    // RELACIÓN CON USUARIO 
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    // RELACIÓN CON RESERVAS
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public string Estado { get; set; } = "Disponible";
}