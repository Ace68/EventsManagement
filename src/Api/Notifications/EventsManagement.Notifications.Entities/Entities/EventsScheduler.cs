using EventsManagement.Notifications.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using EventsManagement.Shared.Enums;
using Muflone.Core;

namespace EventsManagement.Notifications.Entities.Entities;

public class EventsScheduler : AggregateRoot
{
    public EventName EventName { get; private set; } = null!;
    public EventVenue EventVenue { get; private set; } = null!;
    public EventDate EventDate { get; private set; } = null!;
    public EventState EventState { get; private set; } = null!;
    
    protected EventsScheduler()
    {
    }
    
    public static EventsScheduler Create(CommunityEventId eventId, EventName eventName, 
        EventVenue eventVenue, EventDate eventDate)
    {
        return new EventsScheduler(eventId, eventName, eventVenue, eventDate);
    }

    private EventsScheduler(CommunityEventId eventId, EventName eventName, EventVenue eventVenue, EventDate eventDate)
    {
        Id  = eventId;
        
        EventName = eventName;
        EventVenue = eventVenue;
        EventDate = eventDate;
        
        EventState = EventState.Created;
    }
}