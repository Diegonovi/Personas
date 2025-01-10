namespace Personas.Models;

public class Persona
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}