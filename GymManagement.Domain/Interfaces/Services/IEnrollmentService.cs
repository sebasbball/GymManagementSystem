using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId);
        Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId);
        Task<Enrollment> EnrollAsync(Enrollment enrollment);
        Task UnenrollAsync(int enrollmentId);
    }
}