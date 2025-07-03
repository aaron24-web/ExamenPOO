using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class ModuleRepository
{
    private readonly ApplicationDbContext _context;
    public ModuleRepository(ApplicationDbContext context) => _context = context;
    
    public async Task<Module?> GetByIdAsync(Guid id) => await _context.Modules.FindAsync(id);
}