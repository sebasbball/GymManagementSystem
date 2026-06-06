using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<Member?> GetByEmailAsync(string email);
        Task<Member?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Member>> GetAllWithDetailsAsync();
    }
}