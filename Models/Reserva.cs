using System.ComponentModel.DataAnnotations;

namespace DeportivoApp.Models;

public class Reserva
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    [Required]
    public int EspacioId { get; set; }
    public Espacio? Espacio { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public TimeSpan HoraInicio { get; set; }

    [Required]
    public TimeSpan HoraFin { get; set; }

    public string Estado { get; set; } = "Programada";
}