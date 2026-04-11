using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using Muflone.Messages.Events;

namespace EventsManagement.Events.SharedKernel.Messages.Events;

public class CommunityEventCreated(CommunityEventId aggregateId,
    EventName eventName,
    EventDescription eventDescription,
    EventVenue eventVenue,
    IEnumerable<EventOrganizer> eventOrganizers,
    EventDate eventDate,
    Guid correlationId) : DomainEvent(aggregateId, correlationId)
{
    public EventName EventName { get; init; } = eventName;
    public EventDescription EventDescription { get; init; } =  eventDescription;
    public EventVenue EventVenue { get; init; } = eventVenue;
    public IEnumerable<EventOrganizer> EventOrganizers { get; init; } = eventOrganizers;
    public EventDate EventDate { get; init; } = eventDate;
}