using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories
{
    // Handles memberships and their status
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(GymDbContext context) : base(context)
        {
        }

        // Get all memberships that belong to a member
        public async Task<IEnumerable<Membership>> GetByMemberAsync(int memberId)
        {
            return await _context.Memberships
                .Where(m => m.MemberId == memberId)
                .ToListAsync();
        }

        // Get the active membership for a member
        public async Task<Membership?> GetActiveMembershipAsync(int memberId)
        {
            return await _context.Memberships
                .FirstOrDefaultAsync(m => m.MemberId == memberId
                    && m.IsActive
                    && m.EndDate >= DateTime.Now);
        }
    }
}