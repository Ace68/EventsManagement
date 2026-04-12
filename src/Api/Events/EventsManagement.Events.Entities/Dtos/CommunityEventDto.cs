using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.Persister;

namespace EventsManagement.Events.Entities.Dtos;

public class CommunityEventDto : DtoBase
{
    public string EventName { get; private set; } = null!;
    public string EventDescription { get; private set; } = null!;
    public string EventVenue { get; private set; } = null!;
    public DateTime EventDate { get; private set; }

    public virtual ICollection<OrganizersDto> EventOrganizers { get; set; } = null!;
    
    protected CommunityEventDto() { }
    
    public static CommunityEventDto CreateCommunityEvent(CommunityEventId eventId,
        EventName eventName, EventDescription eventDescription, EventVenue eventVenue, 
        EventDate eventDate) 
        => new (new Guid(eventId.Value), eventName.Value, eventDescription.Value, eventVenue.Value, eventDate.Value);

    private CommunityEventDto(Guid eventId, string eventName, string eventDescription, string eventVenue,
        DateTime eventDate)
    {
        Id = eventId;
        
        EventName = eventName;
        EventDescription = eventDescription;
        EventVenue = eventVenue;
        EventDate = eventDate;
    }
    
    public void AddEventOrganizers(IEnumerable<OrganizersDto> organizers) => EventOrganizers = organizers.ToList();
}