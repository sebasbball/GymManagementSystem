namespace GymManagement.Domain.Entities
{
    // Base class for all entities — adds Id and timestamps
    public abstract class AuditBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}