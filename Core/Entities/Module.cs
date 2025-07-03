namespace EducationalPlatformApi.Core.Entities;

public class Module
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    private readonly List<Lesson> _lessons = new();
    public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();
    
    private Module() 
    {
        Title = null!;
    }

    public Module(string title)
    {
        Title = title;
    }
    
    // El método "internal void AddLesson(string lessonTitle)" puede ser eliminado.
}