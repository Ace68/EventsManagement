namespace EventsManagement.Shared.ExternalContracts;

public class EventsSchedulerJson
{
    public string EventId { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string EventVenue { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string EventState { get; set; } = string.Empty;
}