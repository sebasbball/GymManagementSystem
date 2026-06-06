namespace GymManagement.Domain.Entities
{
    // Represents a gym trainer
    public class Trainer : AuditBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;

        // A trainer can teach many classes
        public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();
    }
}