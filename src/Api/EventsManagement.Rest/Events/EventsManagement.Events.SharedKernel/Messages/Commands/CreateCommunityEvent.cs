using EventsManagement.Events.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace EventsManagement.Events.SharedKernel.Messages.Commands;

public class CreateCommunityEvent(CommunityEventId aggregateId,
    EventName eventName,
    EventDescription eventDescription,
    EventVenue eventVenue,
    IEnumerable<EventOrganizer> eventOrganizers,
    EventDate eventDate,
    Guid correlationId) : Command(aggregateId, correlationId)
{
    public EventName EventName { get; init; } = eventName;
    public EventDescription EventDescription { get; init; } =  eventDescription;
    public EventVenue EventVenue { get; init; } = eventVenue;
    public IEnumerable<EventOrganizer> EventOrganizers { get; init; } = eventOrganizers;
    public EventDate EventDate { get; init; } = eventDate;
}