using System.Text.Json;
using Personas.Models;
using Personas.Repository;
using StackExchange.Redis;

namespace Personas.Service;

public class PersonaService:IPersonasService
{
    private readonly IPersonasRepository _repository;
    private readonly ILogger _logger;
    private readonly IDatabase _cache;
    
    public PersonaService(
        IPersonasRepository repository, 
        ILogger<PersonaService> logger,
        IConnectionMultiplexer redis
    )
    {
        _repository = repository;
        _logger = logger;
        _cache = redis.GetDatabase();
    }
    
    public async Task<List<Persona>> GetAllAsync()
    {
        _logger.LogInformation("Getting all personas.");
        return await _repository.GetAllAsync();
    }

    public async Task<Persona?> CreateAsync(Persona persona)
    {
        _logger.LogInformation($"Creating persona: {persona.Nombre}.");
        return await _repository.AddAsync(persona);
    }

    public async Task<Persona?> DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting persona with id: {id}.");
        return await _repository.DeleteAsync(id);
    }
    
    public async Task<Persona?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting persona with id: {id}.");
        var json = await _cache.StringGetAsync(id.ToString());
        
        if (!json.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<Persona>(json);
        }

        _logger.LogInformation($"Persona with id: {id} not found in cache, fetching from database.");
        var persona = await _repository.GetByIdAsync(id);
        if (persona == null)
        {
            _logger.LogInformation($"Persona with id: {id} not found in database.");
            return null;
        }
        await _cache.StringSetAsync(id.ToString(), JsonSerializer.Serialize(persona));
        return persona;
    }
}