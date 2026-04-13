using System.ComponentModel.DataAnnotations;

namespace EventsManagement.Shared.ExternalContracts;

public class CreateCommunityEventJson
{
    [Required]
    public string EventName { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public DateTime Date { get; set; } = DateTime.MaxValue;
    [Required]
    public string Venue { get; set; } = string.Empty;
    [Required]
    public IEnumerable<string> Organizers { get; set; } = [];
}