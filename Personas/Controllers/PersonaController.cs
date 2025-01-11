using Microsoft.AspNetCore.Mvc;
using Personas.Models;
using Personas.Service;

namespace Personas.Controller;

[Route("persona")]
[ApiController]
public class PersonaController : ControllerBase
{
    private readonly IPersonasService _personaService;
    
    public PersonaController(IPersonasService personaService)
    {
        _personaService = personaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Persona>>> GetAll()
    {
        return await _personaService.GetAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> Create(Persona persona)
    {
        var createdPersona = await _personaService.CreateAsync(persona);
        if (createdPersona == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new { id = createdPersona.Id }, createdPersona);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Persona>> Delete(int id)
    {
        var persona = await _personaService.DeleteAsync(id);
        if (persona == null)
            return NotFound();

        return persona;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetById(int id)
    {
        var persona = await _personaService.GetByIdAsync(id);
        if (persona == null)
            return NotFound();

        return persona;
    }
}