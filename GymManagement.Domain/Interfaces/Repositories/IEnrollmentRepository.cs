using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId);
        Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId);
        Task<bool> ExistsByMemberAndClassAsync(int memberId, int gymClassId);
        Task<IEnumerable<Enrollment>> GetByMemberWithDetailsAsync(int memberId);
        Task<IEnumerable<Enrollment>> GetByClassWithDetailsAsync(int gymClassId);
    }
}