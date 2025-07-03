using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;

namespace EducationalPlatformApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateInstructorDto, Instructor>();
        CreateMap<UpdateInstructorDto, Instructor>();
        CreateMap<Instructor, UpdateInstructorDto>(); // Para mapeo bidireccional

        CreateMap<CreateCourseDto, Course>();
        CreateMap<CreateModuleDto, Module>();
        CreateMap<CreateLessonDto, Lesson>();
    }
}