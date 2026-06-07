using GymManagement.Domain.Enums;

namespace GymManagement.API.DTOs.Request
{
    public class MembershipRequestDTO
    {
        public int MemberId { get; set; }
        public MembershipType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}