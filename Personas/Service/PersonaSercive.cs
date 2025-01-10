using Personas.Models;

namespace Personas.Service;

public class PersonaSercive:IPersonasService
{
    
    private const string CacheKeyPrefix = "Persona_"; 
    private readonly ILogger _logger;
    
    public Task<List<Persona>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Persona?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Persona> CreateAsync(Persona personas)
    {
        throw new NotImplementedException();
    }

    public Task<Persona> UpdateAsync(int id, Persona persona)
    {
        throw new NotImplementedException();
    }

    public Task<Persona> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}