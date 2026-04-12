using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.Persister;

namespace EventsManagement.Events.Entities.Dtos;

public class OrganizersDto : DtoBase
{
    public Guid EventId { get; set; }
    public string OrganizerName { get; set; } = null!;
    
    public virtual CommunityEventDto CommunityEvent { get; set; } = null!;
    
    protected OrganizersDto()
    {
    }

    internal static OrganizersDto CreateOrganizer(CommunityEventId eventId, EventOrganizer organizer) 
        => new (new Guid(eventId.Value), organizer.Value);
    
    private OrganizersDto(Guid eventId, string organizerName)
    {
        Id = Guid.CreateVersion7();
        
        EventId = eventId;
        OrganizerName = organizerName;
    }
}