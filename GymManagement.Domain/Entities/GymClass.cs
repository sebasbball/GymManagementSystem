using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities
{
    // Represents a gym class session
    public class GymClass : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public DateTime ScheduledAt { get; set; }
        public ClassStatus Status { get; set; } = ClassStatus.Scheduled;

        // FK to trainer
        public int TrainerId { get; set; }

        // Navigation property
        public Trainer Trainer { get; set; } = null!;

        // A class can have many enrollments
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}