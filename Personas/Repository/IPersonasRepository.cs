using Personas.Models;

namespace Personas.Repository;

public interface IPersonasRepository
{
    Task<List<Persona>> GetAllAsync();
    Task<Persona?> GetByIdAsync(int id);
    Task<Persona?> AddAsync(Persona persona);
    Task<Persona?> DeleteAsync(int id);
}