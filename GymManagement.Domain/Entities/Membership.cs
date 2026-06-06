using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities
{
    // Represents a membership plan assigned to a member
    public class Membership : AuditBase
    {
        public int MemberId { get; set; }
        public MembershipType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Member Member { get; set; } = null!;
    }
}