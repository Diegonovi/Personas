using Microsoft.EntityFrameworkCore;
using Personas.Models;

namespace Personas.Database;

public class PersonasDbContext(DbContextOptions<PersonasDbContext> options)
    : DbContext(options)
    {
    public DbSet<Persona> Personas { get; set; }
    
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