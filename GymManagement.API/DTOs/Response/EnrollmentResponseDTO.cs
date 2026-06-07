namespace GymManagement.API.DTOs.Response
{
    public class EnrollmentResponseDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberFullName { get; set; } = string.Empty;
        public int GymClassId { get; set; }
        public string GymClassName { get; set; } = string.Empty;
        public string TrainerFullName { get; set; } = string.Empty;
        public DateTime EnrolledAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}