using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class LessonRepository
{
    private readonly ApplicationDbContext _context;
    public LessonRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
    }
}