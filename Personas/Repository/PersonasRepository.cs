using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Models;

namespace Personas.Repository;

public class PersonasRepository : IPersonasRepository
{
    private readonly PersonasDbContext _context;
    
    public PersonasRepository(PersonasDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Persona>> GetAllAsync()
    {
        return await _context.Personas.ToListAsync();
    }

    public async Task<Persona?> GetByIdAsync(int id)
    {
        return await _context.Personas.FindAsync(id);
    }

    public async Task<Persona?> AddAsync(Persona persona)
    {
        _context.Personas.Add(persona);
        return await _context.SaveChangesAsync() > 0? persona : null;
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
}