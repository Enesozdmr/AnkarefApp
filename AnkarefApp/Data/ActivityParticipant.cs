namespace AnkarefApp.Data;

public class ActivityParticipant
{
    public Guid ActivityId { get; set; }
    public ActivityTable? ActivityTable { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public bool IsAccepted { get; set; }
}