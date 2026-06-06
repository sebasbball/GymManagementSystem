using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        Task<IEnumerable<Membership>> GetByMemberAsync(int memberId);
        Task<Membership?> GetActiveMembershipAsync(int memberId);
    }
}