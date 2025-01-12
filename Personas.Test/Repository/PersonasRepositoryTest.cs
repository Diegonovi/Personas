using Testcontainers.MySql;
using Microsoft.EntityFrameworkCore;
using Personas.Database;
using Personas.Models;

namespace Personas.Test.Repository
{
    public class PersonasRepositoryTest
    {
        private MySqlContainer _mySqlContainer;
        private PersonasDbContext _dbContext;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var initFile = Path.GetFullPath("../../../Database/Init/init.sql");

            _mySqlContainer = new MySqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("root")
                .WithPassword("password")
                .WithBindMount(initFile, "/docker-entrypoint-initdb.d/init.sql") // Mount init.sql
                .Build();

            await _mySqlContainer.StartAsync();
            
            var options = new DbContextOptionsBuilder<PersonasDbContext>()
                .UseMySql(_mySqlContainer.GetConnectionString(), 
                    new MySqlServerVersion(new Version(8, 0, 29)))
                .Options;

            _dbContext = new PersonasDbContext(options);
        }

        [Test]
        public async Task GetPersonas_ReturnsAllPersonas()
        {
            // Arrange
            await _dbContext.AddAsync(new Persona { Nombre = "John Doe" });
            await _dbContext.AddAsync(new Persona { Nombre = "Jane Doe" });
            await _dbContext.SaveChangesAsync();
            
            // Act
            var personas = await _dbContext.Personas.ToListAsync();

            // Assert
            Assert.That(2 == personas.Count);
            Assert.That("John Doe" == personas[0].Nombre);
        }
        
        [Test]
        public async Task GetPersonaById_ReturnsPersonaById()
        {
            // Arrange
            var persona = await _dbContext.AddAsync(new Persona { Nombre = "John Doe" });
            await _dbContext.SaveChangesAsync();
            
            // Act
            var retrievedPersona = await _dbContext.Personas.FindAsync(persona.Entity.Id);

            // Assert
            Assert.That(persona.Entity.Id == retrievedPersona.Id);
            Assert.That("John Doe" == retrievedPersona.Nombre);
            
            // Cleanup
            _dbContext.Remove(persona.Entity);
            await _dbContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task AddPersona_AddsPersona()
        {
            // Arrange
            var newPersona = new Persona { Nombre = "John Doe" };
            
            // Act
            await _dbContext.AddAsync(newPersona);
            await _dbContext.SaveChangesAsync();

            // Assert
            var retrievedPersona = await _dbContext.Personas.FindAsync(newPersona.Id);
            Assert.That(newPersona.Id == retrievedPersona.Id);
            Assert.That("John Doe" == retrievedPersona.Nombre);
            
            // Cleanup
            _dbContext.Remove(newPersona);
            await _dbContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task DeletePersona_RemovesPersona()
        {
            // Arrange
            var persona = await _dbContext.AddAsync(new Persona { Nombre = "John Doe" });
            await _dbContext.SaveChangesAsync();
            
            // Act
            _dbContext.Remove(persona.Entity);
            await _dbContext.SaveChangesAsync();

            // Assert
            var retrievedPersona = await _dbContext.Personas.FindAsync(persona.Entity.Id);
            Assert.That(null == retrievedPersona);
        }

        [OneTimeTearDown]
        public async Task OneTimeTeardown()
        {
            // Clean up the container
            await _mySqlContainer.DisposeAsync();
            await _dbContext.DisposeAsync();
        }
    }
}
