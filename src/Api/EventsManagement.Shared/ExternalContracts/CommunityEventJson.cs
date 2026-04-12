namespace EventsManagement.Shared.ExternalContracts;

public class CommunityEventJson
{
    public string EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.MaxValue;
    public string Venue { get; set; } = string.Empty;
    public IEnumerable<string> Organizers { get; set; } = [];
}