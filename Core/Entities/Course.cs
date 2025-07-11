namespace EducationalPlatformApi.Core.Entities;

public class Course
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsPublished { get; private set; } = false;
    
    private readonly List<Module> _modules = new();
    public IReadOnlyList<Module> Modules => _modules.AsReadOnly();

    private readonly List<Instructor> _instructors = new();
    public IReadOnlyList<Instructor> Instructors => _instructors.AsReadOnly();

    private Course() 
    {
        Title = null!;
        Description = null!;
    }

    public Course(string title, string description)
    {
        Title = title;
        Description = description;
    }

    // --- Métodos de modificación del Curso ---
    public void UpdateDetails(string title, string description)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot update a published course.");
        Title = title;
        Description = description;
    }

    public void AddInstructor(Instructor instructor)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot add an instructor to a published course.");
        if (!_instructors.Any(i => i.Id == instructor.Id))
        {
            _instructors.Add(instructor);
        }
    }

    public void RemoveInstructor(Instructor instructor)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot remove an instructor from a published course.");
        var existingInstructor = _instructors.FirstOrDefault(i => i.Id == instructor.Id);
        if (existingInstructor != null)
        {
            _instructors.Remove(existingInstructor);
        }
    }

    // --- Métodos de modificación de Hijos ---
    public void AddModule(string moduleTitle)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot add modules to a published course.");
        var module = new Module(moduleTitle) { CourseId = this.Id, Course = this };
        _modules.Add(module);
    }
    
    // El método AddLessonToModule se ha eliminado de aquí para corregir el error.
    
    // --- Lógica de Publicación ---
    public void Publish()
    {
        if (IsPublished) throw new InvalidOperationException("Course is already published.");
        IsPublished = true;
    }
}