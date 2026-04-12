using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using Muflone.Core;

namespace EventsManagement.Events.Entities.Entities;

public class CommunityEvent : AggregateRoot
{
    public EventName EventName{ get; private set; } = null!;
    public EventDescription EventDescription{ get; private set; } = null!;
    public EventVenue EventVenue{ get; private set; } = null!;
    public IEnumerable<EventOrganizer> EventOrganizers{ get; private set; } = [];
    public EventDate EventDate  { get; private set; } = null!;
    
    protected CommunityEvent()
    {
    }

    public static CommunityEvent Create(CommunityEventId eventId, EventName eventName, EventDescription eventDescription,
        EventVenue eventVenue, IEnumerable<EventOrganizer> eventOrganizers, EventDate eventDate)
    {
        return new CommunityEvent(eventId, eventName, eventDescription, eventVenue, eventOrganizers, eventDate);
    }

    private CommunityEvent(CommunityEventId eventId, EventName eventName, EventDescription eventDescription,
        EventVenue eventVenue, IEnumerable<EventOrganizer> eventOrganizers, EventDate eventDate)
    {
        Id = eventId;

        EventName = eventName;
        EventDescription = eventDescription;
        EventVenue = eventVenue;
        EventOrganizers = eventOrganizers;
        EventDate = eventDate;
    }
}