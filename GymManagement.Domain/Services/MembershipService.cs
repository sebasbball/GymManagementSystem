using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services
{
    // Handles all business logic related to memberships
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<MembershipService> _logger;

        public MembershipService(
            IMembershipRepository membershipRepository,
            IMemberRepository memberRepository,
            ILogger<MembershipService> logger)
        {
            _membershipRepository = membershipRepository;
            _memberRepository = memberRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Membership>> GetByMemberAsync(int memberId)
        {
            var exists = await _memberRepository.ExistsAsync(memberId);
            if (!exists)
                throw new KeyNotFoundException($"Member with ID {memberId} not found.");

            return await _membershipRepository.GetByMemberAsync(memberId);
        }

        public async Task<Membership?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving membership with ID: {MembershipId}", id);
            return await _membershipRepository.GetByIdAsync(id);
        }

        public async Task<Membership> CreateAsync(Membership membership)
        {
            // Member must exist before creating a membership
            var member = await _memberRepository.GetByIdAsync(membership.MemberId);
            if (member == null)
                throw new KeyNotFoundException(
                    $"Member with ID {membership.MemberId} not found.");

            // End date must come after start date
            if (membership.EndDate <= membership.StartDate)
                throw new InvalidOperationException(
                    "End date must be after start date.");

            // Check if member already has an active membership
            var activeMembership = await _membershipRepository
                .GetActiveMembershipAsync(membership.MemberId);
            if (activeMembership != null)
                throw new InvalidOperationException(
                    "This member already has an active membership.");

            membership.IsActive = true;

            _logger.LogInformation(
                "Creating {Type} membership for member {MemberId}",
                membership.Type, membership.MemberId);

            return await _membershipRepository.CreateAsync(membership);
        }

        public async Task UpdateAsync(int id, Membership membership)
        {
            var existing = await _membershipRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Membership with ID {id} not found.");

            if (membership.EndDate <= membership.StartDate)
                throw new InvalidOperationException(
                    "End date must be after start date.");

            existing.StartDate = membership.StartDate;
            existing.EndDate = membership.EndDate;
            existing.IsActive = membership.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating membership with ID: {MembershipId}", id);
            await _membershipRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _membershipRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Membership with ID {id} not found.");

            _logger.LogInformation("Deleting membership with ID: {MembershipId}", id);
            await _membershipRepository.DeleteAsync(id);
        }
    }
}