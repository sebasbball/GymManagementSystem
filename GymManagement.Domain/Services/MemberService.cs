using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services
{
    // Handles all business logic related to members
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<MemberService> _logger;

        public MemberService(
            IMemberRepository memberRepository,
            ILogger<MemberService> logger)
        {
            _memberRepository = memberRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all members");
            return await _memberRepository.GetAllWithDetailsAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving member with ID: {MemberId}", id);
            return await _memberRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Member> CreateAsync(Member member)
        {
            // Check if email is already taken
            var existing = await _memberRepository.GetByEmailAsync(member.Email);
            if (existing != null)
                throw new InvalidOperationException(
                    $"A member with email '{member.Email}' already exists.");

            member.JoinDate = DateTime.UtcNow;

            _logger.LogInformation("Creating member: {FirstName} {LastName}",
                member.FirstName, member.LastName);

            return await _memberRepository.CreateAsync(member);
        }

        public async Task UpdateAsync(int id, Member member)
        {
            var existing = await _memberRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Member with ID {id} not found.");

            existing.FirstName = member.FirstName;
            existing.LastName = member.LastName;
            existing.Phone = member.Phone;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating member with ID: {MemberId}", id);
            await _memberRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _memberRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Member with ID {id} not found.");

            _logger.LogInformation("Deleting member with ID: {MemberId}", id);
            await _memberRepository.DeleteAsync(id);
        }
    }
}