using Personas.Models;

namespace Personas.Service;

public interface IPersonasService
{
    public Task<List<Persona>> GetAllAsync();
    public Task<Persona?> GetByIdAsync(int id);
    public Task<Persona> CreateAsync(Persona personas);
    public Task<Persona> UpdateAsync(int id, Persona persona);
    public Task<Persona> DeleteAsync(int id);
}