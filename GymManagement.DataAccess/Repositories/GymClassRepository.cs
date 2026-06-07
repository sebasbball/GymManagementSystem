using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories
{
    // Handles database operations for gym classes
    public class GymClassRepository : GenericRepository<GymClass>, IGymClassRepository
    {
        public GymClassRepository(GymDbContext context) : base(context)
        {
        }

        // Get class with trainer and enrolled members
        public async Task<GymClass?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.GymClasses
                .Include(c => c.Trainer)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Member)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Get all classes with their trainer info
        public async Task<IEnumerable<GymClass>> GetAllWithDetailsAsync()
        {
            return await _context.GymClasses
                .Include(c => c.Trainer)
                .Include(c => c.Enrollments)
                .ToListAsync();
        }

        // Get all classes assigned to a specific trainer
        public async Task<IEnumerable<GymClass>> GetByTrainerAsync(int trainerId)
        {
            return await _context.GymClasses
                .Include(c => c.Trainer)
                .Where(c => c.TrainerId == trainerId)
                .ToListAsync();
        }

        // Count how many members are enrolled in a class
        public async Task<int> GetEnrollmentCountAsync(int gymClassId)
        {
            return await _context.Enrollments
                .CountAsync(e => e.GymClassId == gymClassId);
        }
    }
}