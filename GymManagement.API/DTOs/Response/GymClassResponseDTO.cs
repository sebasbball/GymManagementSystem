using GymManagement.Domain.Enums;

namespace GymManagement.API.DTOs.Response
{
    public class GymClassResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public DateTime ScheduledAt { get; set; }
        public ClassStatus Status { get; set; }
        public int TrainerId { get; set; }
        public string TrainerFullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}