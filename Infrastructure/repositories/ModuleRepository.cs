using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class ModuleRepository
{
    private readonly ApplicationDbContext _context;
    public ModuleRepository(ApplicationDbContext context) => _context = context;
    
    // --- MÉTODO NUEVO ---
    // Este método añade un módulo y guarda los cambios inmediatamente.
    public async Task AddAsync(Module module)
    {
        await _context.Modules.AddAsync(module);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Module?> GetByIdAsync(Guid id) => await _context.Modules.FindAsync(id);
}