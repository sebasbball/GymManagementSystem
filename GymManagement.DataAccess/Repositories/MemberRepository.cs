using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories
{
    // Handles all database operations related to members
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext context) : base(context)
        {
        }

        // Find a member by their email address
        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Email == email);
        }

        // Get member with their memberships loaded
        public async Task<Member?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Members
                .Include(m => m.Memberships)  // plural — it's a collection
                .Include(m => m.Enrollments)
                    .ThenInclude(e => e.GymClass)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // Get all members with their memberships loaded
        public async Task<IEnumerable<Member>> GetAllWithDetailsAsync()
        {
            return await _context.Members
                .Include(m => m.Memberships)  // plural — it's a collection
                .ToListAsync();
        }
    }
}