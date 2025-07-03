namespace EducationalPlatformApi.Core.Entities;

public class Lesson
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    private Lesson() { }

    public Lesson(string title)
    {
        Title = title;
    }
}