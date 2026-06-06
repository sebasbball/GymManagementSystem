using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories
{
    public interface IGymClassRepository : IGenericRepository<GymClass>
    {
        Task<GymClass?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<GymClass>> GetAllWithDetailsAsync();
        Task<IEnumerable<GymClass>> GetByTrainerAsync(int trainerId);
        Task<int> GetEnrollmentCountAsync(int gymClassId);
    }
}