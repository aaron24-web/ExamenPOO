namespace EducationalPlatformApi.Core.Entities;

public class Course
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsPublished { get; private set; } = false;
    public List<Module> Modules { get; private set; } = new();
    public List<Instructor> Instructors { get; private set; } = new();

    private Course() { }

    public Course(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public void AddModule(Module module)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot add modules to a published course.");
        Modules.Add(module);
    }

    public void Publish()
    {
        if (IsPublished) throw new InvalidOperationException("Course is already published.");
        IsPublished = true;
    }
}