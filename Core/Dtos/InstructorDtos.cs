namespace EducationalPlatformApi.Core.DTOs;

public class CreateInstructorDto
{
    public required string Name { get; set; }
    public string? Bio { get; set; }
}

public class UpdateInstructorDto
{
    public string? Name { get; set; }
    public string? Bio { get; set; }
}