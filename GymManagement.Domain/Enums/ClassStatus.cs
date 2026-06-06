namespace GymManagement.Domain.Enums
{
    // Tracks the current state of a gym class
    public enum ClassStatus
    {
        Scheduled = 0,
        InProgress = 1,
        Finished = 2,
        Cancelled = 3
    }
}