using System.ComponentModel.DataAnnotations;

namespace EventsManagement.Shared.ExternalContracts;

public class AddCommunityEventToSchedulerJson
{
    [Required]
    public string EventId { get; set; } = string.Empty;
    [Required]
    public string EventName { get; set; } = string.Empty;
    [Required]
    public string EventVenue { get; set; } = string.Empty;
    [Required]
    public DateTime EventDate { get; set; }
}