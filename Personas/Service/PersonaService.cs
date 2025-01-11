using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Models;

namespace Personas.Service;

public class PersonaService:IPersonasService
{
    private readonly PersonasDbContext _context;
    private const string CacheKeyPrefix = "Persona_"; 
    private readonly ILogger _logger;
    
    public PersonaService(PersonasDbContext context, ILogger<PersonaService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<Persona>> GetAllAsync()
    {
        return await _context.Personas.ToListAsync();
    }

    public async Task<Persona?> CreateAsync(Persona personas)
    {
        _context.Personas.Add(personas);
        return await _context.SaveChangesAsync() > 0? personas : null;
    }

    public async Task<Persona?> DeleteAsync(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona != null)
        {
            _context.Personas.Remove(persona);
            return await _context.SaveChangesAsync() > 0? persona : null;
        }
        return null;
    }
    
    public async Task<Persona?> GetByIdAsync(int id)
    {
        return await _context.Personas.FindAsync(id);
    }
}