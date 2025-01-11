using Personas.Models;

namespace Personas.Service;

public interface IPersonasService
{
    public Task<List<Persona>> GetAllAsync();
    public Task<Persona?> CreateAsync(Persona personas);
    public Task<Persona?> DeleteAsync(int id);
    public Task<Persona?> GetByIdAsync(int id);
}