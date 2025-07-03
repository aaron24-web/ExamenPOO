namespace EducationalPlatformApi.Core.Entities;

public class Instructor
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Bio { get; set; }
    
    private readonly List<Course> _courses = new();
    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    // Constructor para AutoMapper y EF Core
    private Instructor() 
    {
        // Solución a la advertencia: Se inicializa la propiedad.
        Name = null!; 
    } 

    public Instructor(string name, string? bio)
    {
        Name = name;
        Bio = bio;
    }
}