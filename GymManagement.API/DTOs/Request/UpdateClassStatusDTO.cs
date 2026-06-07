using GymManagement.Domain.Enums;

namespace GymManagement.API.DTOs.Request
{
    public class UpdateClassStatusDTO
    {
        public ClassStatus Status { get; set; }
    }
}