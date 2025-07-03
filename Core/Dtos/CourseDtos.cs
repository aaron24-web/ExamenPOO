namespace EducationalPlatformApi.Core.DTOs;

public class CreateCourseDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public List<Guid> InstructorIds { get; set; } = new();
}

public class CreateModuleDto
{
    public required string Title { get; set; }
}

public class CreateLessonDto
{
    public required string Title { get; set; }
}