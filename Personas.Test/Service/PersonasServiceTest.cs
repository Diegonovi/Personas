using Microsoft.Extensions.Logging;
using Moq;
using Personas.Models;
using Personas.Repository;
using Personas.Service;
using StackExchange.Redis;

namespace Personas.Test.Service;

public class PersonasServiceTest
{
    private Mock<IPersonasRepository> _repository;
    private Mock<IConnectionMultiplexer> _connection;
    private PersonaService _personaService;

    private Mock<IDatabase> _redis;
    
    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IPersonasRepository>();
        _connection = new Mock<IConnectionMultiplexer>();
        _redis = new Mock<IDatabase>();
        
        _connection.Setup(connection => connection.GetDatabase(
                It.IsAny<int>(), It.IsAny<object>()
            )
        ).Returns(_redis.Object); // Para que devuelva el objeto IDatabase que queremos moquear
        
        _personaService = new PersonaService(
            _repository.Object, 
            Mock.Of<ILogger<PersonaService>>(),
            _connection.Object
        );
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllPersonas()
    {
        // Arrange
        _repository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Persona>
            {
                new Persona { Id = 1, Nombre = "John Doe" }
            });
        
        // Act
        var result = await _personaService.GetAllAsync();

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result.Count > 0);
                Assert.That(1 == result.Count);
                Assert.That("John Doe" == result[0].Nombre);
            }
        );
    }

    [Test]
    public async Task GetByIdAsync_ReturnsPersonaById_WhenItIsInTheCache()
    {
        // Arrange
        _redis.Setup(redis => redis.StringGetAsync("1", It.IsAny<CommandFlags>()))
            .ReturnsAsync("{\"Id\":1,\"Nombre\":\"John Doe\"}\n");
        
        // Act
        var result = await _personaService.GetByIdAsync(1);

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result != null);
                Assert.That(1 == result.Id);
                Assert.That("John Doe" == result.Nombre);
            }
        );
    }

    [Test]
    public async Task GetByIdAsync_ReturnsPersonaById_WhenItIsNotInTheCache()
    {
        // Arrange
        _repository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
           .ReturnsAsync(new Persona { Id = 1, Nombre = "John Doe" });
        
        _redis.Setup(redis => redis.StringGetAsync("1", It.IsAny<CommandFlags>()))
           .ReturnsAsync(RedisValue.Null);
        
        // Act
        var result = await _personaService.GetByIdAsync(1);

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result!= null);
                Assert.That(1 == result.Id);
                Assert.That("John Doe" == result.Nombre);
            }
        );
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNullWhenPersonIsNotFound()
    {
        // Arrange
        _repository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
           .ReturnsAsync((Persona?)null);
        
        _redis.Setup(redis => redis.StringGetAsync("1", It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);
        // Act
        var result = await _personaService.GetByIdAsync(1);

        // Assert
        Assert.That(result == null);
    }
    
    [Test]
    public async Task CreateAsync_AddsPersona()
    {
        // Arrange
        var newPersona = new Persona { Id = 1, Nombre = "John Doe" };
        _repository.Setup(repo => repo.AddAsync(It.Is<Persona>(p => p.Id == newPersona.Id && p.Nombre == newPersona.Nombre)))
           .ReturnsAsync(newPersona);

        // Act
        var result = await _personaService.CreateAsync(newPersona);

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result!= null);
                Assert.That(1 == result.Id);
                Assert.That("John Doe" == result.Nombre);
            }
        );
    }
    
    [Test]
    public async Task DeleteAsync_RemovesPersona()
    {
        // Arrange
        _repository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
           .ReturnsAsync(new Persona { Id = 1, Nombre = "John Doe" });

        // Act
        var result = await _personaService.DeleteAsync(1);

        // Assert
        Assert.Multiple(() =>
            {
                Assert.That(result!= null);
                Assert.That(1 == result.Id);
                Assert.That("John Doe" == result.Nombre);
            }
        );
    }

    [Test]
    public async Task DeleteAsync_ReturnsNullWhenPersonIsNotFound()
    {
        // Arrange
        _repository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((Persona?)null);

        // Act
        var result = await _personaService.DeleteAsync(1);

        // Assert
        Assert.That(result == null);
    }
}