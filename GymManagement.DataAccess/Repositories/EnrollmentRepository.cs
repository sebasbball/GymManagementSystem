using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories
{
    // Handles enrollments between members and classes
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(GymDbContext context) : base(context)
        {
        }

        // Get all enrollments for a specific member
        public async Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId)
        {
            return await _context.Enrollments
                .Where(e => e.MemberId == memberId)
                .ToListAsync();
        }

        // Get all enrollments for a specific class
        public async Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId)
        {
            return await _context.Enrollments
                .Where(e => e.GymClassId == gymClassId)
                .ToListAsync();
        }

        // Check if a member is already enrolled in a specific class
        public async Task<bool> ExistsByMemberAndClassAsync(int memberId, int gymClassId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.MemberId == memberId && e.GymClassId == gymClassId);
        }

        // Get enrollments for a member with class details loaded
        public async Task<IEnumerable<Enrollment>> GetByMemberWithDetailsAsync(int memberId)
        {
            return await _context.Enrollments
                .Include(e => e.GymClass)
                    .ThenInclude(c => c.Trainer)
                .Where(e => e.MemberId == memberId)
                .ToListAsync();
        }

        // Get enrollments for a class with member details loaded
        public async Task<IEnumerable<Enrollment>> GetByClassWithDetailsAsync(int gymClassId)
        {
            return await _context.Enrollments
                .Include(e => e.Member)
                .Where(e => e.GymClassId == gymClassId)
                .ToListAsync();
        }
    }
}