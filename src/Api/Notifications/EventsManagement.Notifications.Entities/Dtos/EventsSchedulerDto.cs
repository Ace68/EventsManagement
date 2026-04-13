using EventsManagement.Notifications.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.Enums;
using EventsManagement.Shared.Persister;

namespace EventsManagement.Notifications.Entities.Dtos;

public class EventsSchedulerDto : DtoBase
{
    public string EventName { get; private set; } = string.Empty;
    public string EventVenue { get; private set; } = string.Empty;
    public DateTime EventDate { get; private set; } = DateTime.MinValue;
    public string EventState { get; private set; } = string.Empty;
    
    protected EventsSchedulerDto()
    {
    }
    
    public static EventsSchedulerDto Create(CommunityEventId eventId, EventName eventName, EventVenue eventVenue, 
        EventDate eventDate, EventState eventState)
    {
        return new EventsSchedulerDto(eventId.Value, eventName.Value, eventVenue.Value, eventDate.Value, eventState.Name);
    }
    
    private EventsSchedulerDto(string eventId, string eventName, string eventVenue, DateTime eventDate, string eventState)
    {
        Id = new Guid(eventId);
        
        EventName = eventName;
        EventVenue = eventVenue;
        EventDate = eventDate;
        EventState = eventState;
    }
}