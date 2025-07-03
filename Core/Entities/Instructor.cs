namespace EducationalPlatformApi.Core.Entities;

public class Instructor
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Bio { get; set; }
    public List<Course> Courses { get; private set; } = new();

    // Constructor para AutoMapper y EF Core
    private Instructor() { } 

    public Instructor(string name, string? bio)
    {
        Name = name;
        Bio = bio;
    }
}