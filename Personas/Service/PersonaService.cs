using System.Text.Json;
using Personas.Models;
using Personas.Repository;
using StackExchange.Redis;

namespace Personas.Service
{
    /// <summary>
    /// Service class for managing personas. This class interacts with the repository and cache to perform CRUD operations on personas.
    /// </summary>
    public class PersonaService : IPersonasService
    {
        private readonly IPersonasRepository _repository;
        private readonly ILogger _logger;
        private readonly IDatabase _cache;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaService"/> class.
        /// </summary>
        /// <param name="repository">The repository to interact with the persona data.</param>
        /// <param name="logger">The logger to log information and errors.</param>
        /// <param name="redis">The connection multiplexer to interact with the Redis cache.</param>
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
        
        /// <summary>
        /// Retrieves all personas from the repository.
        /// </summary>
        /// <returns>A list of personas.</returns>
        public async Task<List<Persona>> GetAllAsync()
        {
            _logger.LogInformation("Getting all personas.");
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Creates a new persona and adds it to the repository.
        /// </summary>
        /// <param name="persona">The persona to be created.</param>
        /// <returns>The created persona.</returns>
        public async Task<Persona?> CreateAsync(Persona persona)
        {
            _logger.LogInformation($"Creating persona: {persona.Nombre}.");
            return await _repository.AddAsync(persona);
        }

        /// <summary>
        /// Deletes a persona by its ID.
        /// </summary>
        /// <param name="id">The ID of the persona to delete.</param>
        /// <returns>The deleted persona, or null if not found.</returns>
        public async Task<Persona?> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting persona with id: {id}.");
            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves a persona by its ID. If not found in the cache, fetches from the database.
        /// </summary>
        /// <param name="id">The ID of the persona to retrieve.</param>
        /// <returns>The persona if found, or null if not found.</returns>
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
}
