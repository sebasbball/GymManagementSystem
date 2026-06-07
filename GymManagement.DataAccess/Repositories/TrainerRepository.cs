using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories
{
    // Handles database operations for trainers
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(GymDbContext context) : base(context)
        {
        }

        // Find a trainer by their email address
        public async Task<Trainer?> GetByEmailAsync(string email)
        {
            return await _context.Trainers
                .FirstOrDefaultAsync(t => t.Email == email);
        }

        // Get all trainers with the classes they teach
        public async Task<IEnumerable<Trainer>> GetAllWithClassesAsync()
        {
            return await _context.Trainers
                .Include(t => t.GymClasses)
                .ToListAsync();
        }
    }
}