using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services
{
    // Handles all business logic for enrolling members into classes
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IGymClassRepository _gymClassRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            IMemberRepository memberRepository,
            IGymClassRepository gymClassRepository,
            IMembershipRepository membershipRepository,
            ILogger<EnrollmentService> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _memberRepository = memberRepository;
            _gymClassRepository = gymClassRepository;
            _membershipRepository = membershipRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId)
        {
            var exists = await _memberRepository.ExistsAsync(memberId);
            if (!exists)
                throw new KeyNotFoundException($"Member with ID {memberId} not found.");

            return await _enrollmentRepository.GetByMemberWithDetailsAsync(memberId);
        }

        public async Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId)
        {
            var exists = await _gymClassRepository.ExistsAsync(gymClassId);
            if (!exists)
                throw new KeyNotFoundException($"Gym class with ID {gymClassId} not found.");

            return await _enrollmentRepository.GetByClassWithDetailsAsync(gymClassId);
        }

        public async Task<Enrollment> EnrollAsync(Enrollment enrollment)
        {
            // Member must exist
            var member = await _memberRepository.GetByIdAsync(enrollment.MemberId);
            if (member == null)
                throw new KeyNotFoundException(
                    $"Member with ID {enrollment.MemberId} not found.");

            // Class must exist
            var gymClass = await _gymClassRepository.GetByIdWithDetailsAsync(enrollment.GymClassId);
            if (gymClass == null)
                throw new KeyNotFoundException(
                    $"Gym class with ID {enrollment.GymClassId} not found.");

            // Class must be in Scheduled status to accept enrollments
            if (gymClass.Status != ClassStatus.Scheduled)
                throw new InvalidOperationException(
                    "Can only enroll in classes with Scheduled status.");

            // Member cannot enroll twice in the same class
            var alreadyEnrolled = await _enrollmentRepository
                .ExistsByMemberAndClassAsync(enrollment.MemberId, enrollment.GymClassId);
            if (alreadyEnrolled)
                throw new InvalidOperationException(
                    "This member is already enrolled in this class.");

            // Check if class still has available spots
            var currentCount = await _gymClassRepository
                .GetEnrollmentCountAsync(enrollment.GymClassId);
            if (currentCount >= gymClass.Capacity)
                throw new InvalidOperationException(
                    "This class is already full. No more spots available.");

            // Member must have an active membership to enroll
            var activeMembership = await _membershipRepository
                .GetActiveMembershipAsync(enrollment.MemberId);
            if (activeMembership == null)
                throw new InvalidOperationException(
                    "The member does not have an active membership.");

            enrollment.EnrolledAt = DateTime.UtcNow;

            _logger.LogInformation(
                "Enrolling member {MemberId} in class {ClassId}",
                enrollment.MemberId, enrollment.GymClassId);

            return await _enrollmentRepository.CreateAsync(enrollment);
        }

        public async Task UnenrollAsync(int enrollmentId)
        {
            var exists = await _enrollmentRepository.ExistsAsync(enrollmentId);
            if (!exists)
                throw new KeyNotFoundException(
                    $"Enrollment with ID {enrollmentId} not found.");

            _logger.LogInformation("Removing enrollment with ID: {EnrollmentId}", enrollmentId);
            await _enrollmentRepository.DeleteAsync(enrollmentId);
        }
    }
}