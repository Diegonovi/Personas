using Microsoft.Extensions.Logging;
using Moq;
using Personas.Models;
using Personas.Repository;
using Personas.Service;

namespace Personas.Test.Service;

public class PersonasServiceTest
{
    private Mock<IPersonasRepository> _mock;
    private PersonaService _personaService;
    
    [SetUp]
    public void Setup()
    {
        _mock = new Mock<IPersonasRepository>();
        _personaService = new PersonaService(_mock.Object, Mock.Of<ILogger<PersonaService>>());
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllPersonas()
    {
        // Arrange
        _mock.Setup(repo => repo.GetAllAsync())
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
    public async Task GetByIdAsync_ReturnsPersonaById()
    {
        // Arrange
        _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Persona { Id = 1, Nombre = "John Doe" });

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
    public async Task GetByIdAsync_ReturnsNullWhenPersonIsNotFound()
    {
        // Arrange
        _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
           .ReturnsAsync((Persona?)null);

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
        _mock.Setup(repo => repo.AddAsync(It.Is<Persona>(p => p.Id == newPersona.Id && p.Nombre == newPersona.Nombre)))
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
        _mock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
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
        _mock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((Persona?)null);

        // Act
        var result = await _personaService.DeleteAsync(1);

        // Assert
        Assert.That(result == null);
    }

}