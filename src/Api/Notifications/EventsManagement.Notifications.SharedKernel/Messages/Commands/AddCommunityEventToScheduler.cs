using EventsManagement.Notifications.SharedKernel.CustomTypes;
using EventsManagement.Shared.DomainIds;
using Muflone.Messages.Commands;

namespace EventsManagement.Notifications.SharedKernel.Messages.Commands;

public class AddCommunityEventToScheduler(CommunityEventId aggregateId, 
    EventName eventName, EventVenue eventVenue, EventDate eventDate) : Command(aggregateId)
{
    public EventName EventName { get; init; } = eventName;
    public EventVenue EventVenue { get; init; } = eventVenue;
    public EventDate EventDate { get; init; } = eventDate;
}