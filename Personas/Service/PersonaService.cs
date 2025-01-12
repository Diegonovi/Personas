using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Models;
using Personas.Repository;

namespace Personas.Service;

public class PersonaService:IPersonasService
{
    private readonly IPersonasRepository _repository;
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
        return await _repository.GetByIdAsync(id);
    }
}