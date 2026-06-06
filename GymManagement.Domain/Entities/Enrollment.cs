namespace GymManagement.Domain.Entities
{
    // Junction table — links Member and GymClass (N:M relationship)
    public class Enrollment : AuditBase
    {
        public int MemberId { get; set; }
        public int GymClassId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Member Member { get; set; } = null!;
        public GymClass GymClass { get; set; } = null!;
    }
}