using Microsoft.EntityFrameworkCore;
using Personas.Models;

namespace Personas.Database;

public class PersonasDBContext(DbContextOptions<PersonasDBContext> options)
    : DbContext(options)
    {
    public DbSet<Persona> Heroes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.Property(e => e.CreatedAt).IsRequired()
                .ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedAt).IsRequired()
                .ValueGeneratedOnUpdate();
        });
    }
}