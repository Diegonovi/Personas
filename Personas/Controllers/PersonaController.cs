using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Models;

namespace Personas.Controllers;

[Route("personas")]
[ApiController]
public class PersonaController : ControllerBase
{
    private readonly PersonasDBContext _context;
    private readonly ILogger<PersonaController> _logger;

    public PersonaController(PersonasDBContext context, ILogger<PersonaController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Persona>>> GetAll()
    {
        _logger.LogDebug("GetAll");
        return Ok(await _context.Personas.Select(p => p).ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> Create(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = persona.Id }, persona);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona == null)
        {
            return NotFound();
        }

        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}