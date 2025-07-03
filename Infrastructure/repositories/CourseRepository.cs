using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class CourseRepository
{
    private readonly ApplicationDbContext _context;
    public CourseRepository(ApplicationDbContext context) => _context = context;
    
    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task<Course?> GetByIdAsync(Guid id) =>
        await _context.Courses
            .Include(c => c.Modules)
            .Include(c => c.Instructors)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }
}