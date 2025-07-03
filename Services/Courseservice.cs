using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Services;

public class CourseService
{
    private readonly CourseRepository _courseRepository;
    private readonly ModuleRepository _moduleRepository;
    private readonly LessonRepository _lessonRepository;
    private readonly InstructorRepository _instructorRepository;
    private readonly IMapper _mapper;

    public CourseService(
        CourseRepository courseRepository,
        ModuleRepository moduleRepository,
        LessonRepository lessonRepository,
        InstructorRepository instructorRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _moduleRepository = moduleRepository;
        _lessonRepository = lessonRepository;
        _instructorRepository = instructorRepository;
        _mapper = mapper;
    }

    public async Task<Course> CreateCourseAsync(CreateCourseDto dto)
    {
        var course = _mapper.Map<Course>(dto);

        if (dto.InstructorIds.Any())
        {
            var instructors = await _instructorRepository.GetByIdsAsync(dto.InstructorIds);
            if (instructors.Count != dto.InstructorIds.Count)
            {
                throw new KeyNotFoundException("One or more instructors were not found.");
            }
            course.Instructors.AddRange(instructors);
        }

        await _courseRepository.AddAsync(course);
        return course;
    }
    
    public async Task AddModuleToCourseAsync(Guid courseId, CreateModuleDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");

        var module = _mapper.Map<Module>(dto);
        course.AddModule(module);

        await _courseRepository.UpdateAsync(course);
    }

    public async Task PublishCourseAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");

        course.Publish();
        await _courseRepository.UpdateAsync(course);
    }
    
    public async Task RemoveInstructorFromCourseAsync(Guid courseId, Guid instructorId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new KeyNotFoundException("Course not found.");

        if (course.IsPublished)
            throw new InvalidOperationException("Cannot remove an instructor from a published course.");

        var instructor = course.Instructors.FirstOrDefault(i => i.Id == instructorId);
        if (instructor == null)
            throw new KeyNotFoundException("Instructor is not assigned to this course.");

        course.Instructors.Remove(instructor);
        await _courseRepository.UpdateAsync(course);
    }

    public async Task AddLessonToModuleAsync(Guid moduleId, CreateLessonDto dto)
    {
        var module = await _moduleRepository.GetByIdAsync(moduleId);
        if (module == null) throw new KeyNotFoundException("Module not found.");

        var course = await _courseRepository.GetByIdAsync(module.CourseId);
        if (course == null || course.IsPublished)
            throw new InvalidOperationException("Cannot add lessons to a module in a published course.");

        var lesson = _mapper.Map<Lesson>(dto);
        lesson.ModuleId = moduleId;

        await _lessonRepository.AddAsync(lesson);
    }
}