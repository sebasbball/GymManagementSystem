namespace GymManagement.Domain.Entities
{
    // Represents a gym member
    public class Member : AuditBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime JoinDate { get; set; }

        // A member can have many memberships
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

        // A member can enroll in many classes
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}