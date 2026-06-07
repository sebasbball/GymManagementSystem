namespace GymManagement.API.DTOs.Request
{
    public class GymClassRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int TrainerId { get; set; }
    }
}