using EventsManagement.Shared.Persister;

namespace EventsManagement.Events.Entities.Dtos;

public class CommunityEventDto : DtoBase
{
    public string EventName { get; set; } = null!;
    public string EventDescription { get; set; } = null!;
    public string EventVenue { get; set; } = null!;
    public DateTime EventDate { get; set; }

    public virtual ICollection<OrganizersDto> EventOrganizers { get; set; } = null!;
}