using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Models;
using Personas.Repository;

namespace Personas.Service;

public class PersonaService:IPersonasService
{
    private readonly IPersonasRepository _repository;
    private const string CacheKeyPrefix = "Persona_"; 
    private readonly ILogger _logger;
    
    public PersonaService(
        IPersonasRepository repository, 
        ILogger<PersonaService> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<List<Persona>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Persona?> CreateAsync(Persona persona)
    {
        return await _repository.AddAsync(persona);
    }

    public async Task<Persona?> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
    
    public async Task<Persona?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}