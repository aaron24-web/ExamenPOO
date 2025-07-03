namespace EducationalPlatformApi.Core.Entities;

public class Module
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public List<Lesson> Lessons { get; private set; } = new();

    private Module() { }

    public Module(string title)
    {
        Title = title;
    }
}